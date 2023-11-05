using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#if (UNITY_IOS || UNITY_TVOS)
using UnityEngine.SocialPlatforms.GameCenter;
#elif UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
#endif
public class LoginUI : MonoBehaviour
{
	[Header("Layout Settings")]

	[SerializeField] float itemSpacing = .5f;
	float itemHeight;

	[Space(20)]
	[Header("Firebase")]
	[SerializeField] GameObject firebaseManagerController;

	[Space(20)]
	[Header("Login Events")]
	[SerializeField] GameObject loginUI;
	[SerializeField] GameObject loadingUI;
	[SerializeField] GameObject loggedInUI;

	[SerializeField] Button openLoginButton;
	[SerializeField] Button closeLoginButton;

	[Space(20)]
	[Header("Scroll View")]
	[SerializeField] ScrollRect scrollRect;


	[Space(20)]
	[Header("Error messages")]
	[SerializeField] TMP_Text ErrorText;

	int newSelectedItemIndex = 0;
	int previousSelectedItemIndex = 0;
	Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

	protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
	  new Dictionary<string, Firebase.Auth.FirebaseUser>();

	[Space(20)]
	[Header("Login params")]
	public InputField email;
	public InputField password;
	protected string displayName = "";

	protected bool signInAndFetchProfile = false;
	// Flag set when a token is being fetched.  This is used to avoid printing the token
	// in IdTokenChanged() when the user presses the get token button.
	private bool fetchingToken = false;
	// Enable / disable password input box.
	// NOTE: In some versions of Unity the password input box does not work in
	// iOS simulators.
	protected Firebase.Auth.FirebaseAuth auth;
	private string authCode;
	void Start()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		AuthenticateToGooglePlayGames();
		return;
#endif
		
		AddLoginEvents();

		//Auto scroll to selected character  in the login
		AutoScrollLoginList(GameDataManager.GetSelectedCharacterIndex());

		FirebaseManager firebaseManagerComponent = firebaseManagerController.GetComponent<FirebaseManager>();
		if (firebaseManagerComponent != null)
		{
			// Wait for FirebaseManager to be initialized before initializing LoginUI
			firebaseManagerComponent.OnFirebaseInitialized += () =>
			{
				auth = firebaseManagerComponent.auth;
				InitializeFirebase(); // This is the LoginUI's InitializeFirebase method
			};

			firebaseManagerComponent.InitializeFirebase();
		}
	}

	// Display user information.
	protected void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel)
	{
		string indent = new String(' ', indentLevel * 2);
		var userProperties = new Dictionary<string, string> {
		{"Display Name", userInfo.DisplayName},
		{"Email", userInfo.Email},
		{"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
		{"Provider ID", userInfo.ProviderId},
		{"User ID", userInfo.UserId}
	  };
		foreach (var property in userProperties)
		{
			if (!String.IsNullOrEmpty(property.Value))
			{
				Debug.Log(String.Format("{0}{1}: {2}", indent, property.Key, property.Value));
			}
		}
	}

	// Display a more detailed view of a FirebaseUser.
	protected void DisplayDetailedUserInfo(Firebase.Auth.FirebaseUser user, int indentLevel)
	{
		string indent = new String(' ', indentLevel * 2);
		DisplayUserInfo(user, indentLevel);
		Debug.Log(String.Format("{0}Anonymous: {1}", indent, user.IsAnonymous));
		Debug.Log(String.Format("{0}Email Verified: {1}", indent, user.IsEmailVerified));
		Debug.Log(String.Format("{0}Phone Number: {1}", indent, user.PhoneNumber));
		var providerDataList = new List<Firebase.Auth.IUserInfo>(user.ProviderData);
		var numberOfProviders = providerDataList.Count;
		if (numberOfProviders > 0)
		{
			for (int i = 0; i < numberOfProviders; ++i)
			{
				Debug.Log(String.Format("{0}Provider Data: {1}", indent, i));
				DisplayUserInfo(providerDataList[i], indentLevel + 2);
			}
		}
	}

	// Track state changes of the auth object.
	void AuthStateChanged(object sender, System.EventArgs eventArgs)
	{
		Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
		Firebase.Auth.FirebaseUser user = null;
		if (senderAuth != null) userByAuth.TryGetValue(senderAuth.App.Name, out user);
		if (senderAuth == auth && senderAuth.CurrentUser != user)
		{
			bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;
			if (!signedIn && user != null)
			{
				Debug.Log("Signed out " + user.UserId);
			}
			user = senderAuth.CurrentUser;
			userByAuth[senderAuth.App.Name] = user;
			if (signedIn)
			{
				Debug.Log("AuthStateChanged Signed in " + user.UserId);
				displayName = user.DisplayName ?? "";
				DisplayDetailedUserInfo(user, 1);
				loggedInUI.SetActive(true);
			}
			else
			{
				loggedInUI.SetActive(false);
			}
		}
	}

	// Track ID token changes.
	void IdTokenChanged(object sender, System.EventArgs eventArgs)
	{
		Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
		if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken)
		{
			senderAuth.CurrentUser.TokenAsync(false).ContinueWithOnMainThread(
			  task => Debug.Log(String.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
		}
	}

	// Handle initialization of the necessary firebase modules:
	protected void InitializeFirebase()
	{
		Debug.Log("Setting up Firebase Auth");
		auth.StateChanged += AuthStateChanged;
		auth.IdTokenChanged += IdTokenChanged;
		AuthStateChanged(this, null);
	}

	// Log the result of the specified task, returning true if the task
	// completed successfully, false otherwise.
	protected bool LogTaskCompletion(Task task, string operation)
	{
		bool complete = false;
		if (task.IsCanceled)
		{
			Debug.Log(operation + " canceled.");
		}
		else if (task.IsFaulted)
		{
			Debug.Log(operation + " encounted an error.");
			foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
			{
				string authErrorCode = "";
				Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
				if (firebaseEx != null)
				{
					authErrorCode = String.Format("AuthError.{0}: ",
					  ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
				}
				Debug.Log(authErrorCode + exception.ToString());
			}
		}
		else if (task.IsCompleted)
		{
			Debug.Log(operation + " completed");
			complete = true;
		}
		return complete;
	}

	// Create a user with the email and password.
	private async Task CreateUserWithEmailAsync()
	{
		try
		{
			await auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text)
				.ContinueWithOnMainThread(task =>
				{
					if (task.IsFaulted)
					{
						Debug.LogError("Failed to create user: " + task.Exception);
					}
					else if (task.IsCompleted)
					{
						Debug.Log("User created successfully!");
						HandleSignInWithAuthResult(task);
					}
				});
		}
		catch (Exception e)
		{
			Debug.LogError("User creation failed with error: " + e.Message);
		}
	}
	// Update the user's display name with the currently selected display name.
	public Task UpdateUserProfileAsync(string newDisplayName = null)
	{
		if (auth.CurrentUser == null)
		{
			Debug.Log("Not signed in, unable to update user profile");
			return Task.FromResult(0);
		}
		displayName = newDisplayName ?? displayName;
		Debug.Log("Updating user profile");
		DisableUI();
		return auth.CurrentUser.UpdateUserProfileAsync(new Firebase.Auth.UserProfile
		{
			DisplayName = displayName,
			PhotoUrl = auth.CurrentUser.PhotoUrl,
		}).ContinueWithOnMainThread(task =>
		{
			EnableUI();
			if (LogTaskCompletion(task, "User profile"))
			{
				DisplayDetailedUserInfo(auth.CurrentUser, 1);
			}
		});
	}

	// Called when a sign-in without fetching profile data completes.
	void HandleSignInWithAuthResult(Task<Firebase.Auth.AuthResult> task)
	{
		EnableUI();
		if (LogTaskCompletion(task, "Sign-in"))
		{
			if (task.Result.User != null && task.Result.User.IsValid())
			{
				DisplayAuthResult(task.Result, 1);
				Debug.Log(String.Format("{0} signed in", task.Result.User.DisplayName));
			}
			else
			{
				Debug.Log("Signed in but User is either null or invalid");
			}
		}
	}

	// Display additional user profile information.
	protected void DisplayProfile<T>(IDictionary<T, object> profile, int indentLevel)
	{
		string indent = new String(' ', indentLevel * 2);
		foreach (var kv in profile)
		{
			var valueDictionary = kv.Value as IDictionary<object, object>;
			if (valueDictionary != null)
			{
				Debug.Log(String.Format("{0}{1}:", indent, kv.Key));
				DisplayProfile<object>(valueDictionary, indentLevel + 1);
			}
			else
			{
				Debug.Log(String.Format("{0}{1}: {2}", indent, kv.Key, kv.Value));
			}
		}
	}

	// Display user information reported
	protected void DisplayAuthResult(Firebase.Auth.AuthResult result, int indentLevel)
	{
		string indent = new String(' ', indentLevel * 2);
		DisplayDetailedUserInfo(result.User, indentLevel);
		var metadata = result.User != null ? result.User.Metadata : null;
		if (metadata != null)
		{
			Debug.Log(String.Format("{0}Created: {1}", indent, metadata.CreationTimestamp));
			Debug.Log(String.Format("{0}Last Sign-in: {1}", indent, metadata.LastSignInTimestamp));
		}
		var info = result.AdditionalUserInfo;
		if (info != null)
		{
			Debug.Log(String.Format("{0}Additional User Info:", indent));
			Debug.Log(String.Format("{0}  User Name: {1}", indent, info.UserName));
			Debug.Log(String.Format("{0}  Provider ID: {1}", indent, info.ProviderId));
			DisplayProfile<string>(info.Profile, indentLevel + 1);
		}
		var credential = result.Credential;
		if (credential != null)
		{
			Debug.Log(String.Format("{0}Credential:", indent));
			Debug.Log(String.Format("{0}  Is Valid?: {1}", indent, credential.IsValid()));
			Debug.Log(String.Format("{0}  Class Type: {1}", indent, credential.GetType()));
			if (credential.IsValid())
			{
				Debug.Log(String.Format("{0}  Provider: {1}", indent, credential.Provider));
			}
		}
	}

	// Sign-in with an email and password.
	public async void SigninWithEmailAsync()
	{

		Debug.Log(String.Format("Attempting to sign in as {0}...", email.text));
		DisableUI();

		if (signInAndFetchProfile)
		{
			await auth.SignInAndRetrieveDataWithCredentialAsync(
			  Firebase.Auth.EmailAuthProvider.GetCredential(email.text, password.text)).ContinueWithOnMainThread(
				HandleSignInWithAuthResult);
		}
		else
		{
			await auth.SignInWithEmailAndPasswordAsync(email.text, password.text)
			  .ContinueWithOnMainThread(HandleSignInWithAuthResult);
		}
	}



	// Attempt to sign in anonymously.
	public async void SigninAnonymouslyAsync()
	{
		Debug.Log("Attempting to sign anonymously...");
		DisableUI();
		await auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(HandleSignInWithAuthResult);
	}

	public void AuthenticateToGooglePlayGames()
	{
#if UNITY_ANDROID
		PlayGamesPlatform.Activate();
        
		PlayGamesPlatform.Instance.Authenticate(success =>
		{
			if (success == SignInStatus.Success)
			{
				Debug.Log("Login with Google Play successful.");
				PlayGamesPlatform.Instance.RequestServerSideAccess(true, serverAuthCode =>
				{
					Debug.Log($"Auth code is {serverAuthCode}");
					Firebase.Auth.Credential credential =
						Firebase.Auth.PlayGamesAuthProvider.GetCredential(serverAuthCode);
					auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task => {
						if (task.IsCanceled) {
							Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
							return;
						}
						if (task.IsFaulted) {
							Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
							return;
						}
						if (task.IsCompleted)
						{
							Debug.Log("User created successfully!");
							HandleSignInWithAuthResult(task);
						}
					});
				});
			}
			else 
			{
				Debug.Log(success.ToString());
				Debug.LogError("Failed to retrieve Google Play auth code.");
				//TODO something else?
			}
		});
#else
        Debug.Log("Google Play Games SDK only works on Android devices. Please build your app to an Android device.");
#endif
	}

	public void AuthenticateToGameCenter()
	{
#if (UNITY_IOS || UNITY_TVOS)
        Social.localUser.Authenticate(success => {
          Debug.Log("Game Center Initialization Complete - Result: " + success);
        });
#else
		Debug.Log("Game Center is not supported on this platform.");
#endif
	}

	// Link the current user with an email / password credential.
	protected Task LinkWithEmailCredentialAsync()
	{
		if (auth.CurrentUser == null)
		{
			Debug.Log("Not signed in, unable to link credential to user.");
			var tcs = new TaskCompletionSource<bool>();
			tcs.SetException(new Exception("Not signed in"));
			return tcs.Task;
		}
		Debug.Log("Attempting to link credential to user...");
		Firebase.Auth.Credential cred =
		  Firebase.Auth.EmailAuthProvider.GetCredential(email.text, password.text);
		return auth.CurrentUser.LinkWithCredentialAsync(cred).ContinueWithOnMainThread(task =>
		{
			if (LogTaskCompletion(task, "Link Credential"))
			{
				DisplayDetailedUserInfo(task.Result.User, 1);
			}
		});
	}

	// Sign out the current user.
	public void SignOut()
	{
		Debug.Log("Signing out.");
		auth.SignOut();
	}

	void AutoScrollLoginList(int itemIndex)
	{
		//scrollRect.verticalNormalizedPosition = 0f; //means scroll to the bottom
		//scrollRect.verticalNormalizedPosition = 1f; //means scroll to the top
		scrollRect.verticalNormalizedPosition = Mathf.Clamp01(1f - itemIndex);
	}

	// Send a password reset email to the current email address.
	public void SendPasswordResetEmail()
	{
		auth.SendPasswordResetEmailAsync(email.text).ContinueWithOnMainThread((authTask) =>
		{
			if (LogTaskCompletion(authTask, "Send Password Reset Email"))
			{
				Debug.Log("Password reset email sent to " + email.text);
			}
		});
	}

	void AddLoginEvents()
	{
		openLoginButton.onClick.RemoveAllListeners();
		openLoginButton.onClick.AddListener(OpenLogin);

		closeLoginButton.onClick.RemoveAllListeners();
		closeLoginButton.onClick.AddListener(CloseLogin);

		scrollRect.onValueChanged.RemoveAllListeners();
		scrollRect.onValueChanged.AddListener(OnLoginListScroll);
	}

	void OnLoginListScroll(Vector2 value)
	{
		float scrollY = value.y;
	}


	void EnableUI()
	{
		loadingUI.SetActive(false);
	}

	void DisableUI()
	{
		loadingUI.SetActive(true);
	}

	void OpenLogin()
	{
		loginUI.SetActive(true);
	}

	void CloseLogin()
	{
		loginUI.SetActive(false);
	}

}
