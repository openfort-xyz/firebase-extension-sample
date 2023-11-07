## Overview

[Firebase](https://firebase.google.com/) is Google's all-encompassing app development platform, providing game developers with an array of backend tools powered by Google Cloud.

Within Unity, we've integrated the Firebase SDK to facilitate backend functionalities. Coupled with the [Google Play Games plugin for Unity](https://github.com/playgameservices/play-games-plugin-for-unity), it ensures a secure and efficient login for Android users.

The integration is further enhanced with the [Openfort Firebase extension](https://extensions.dev/extensions/openfort/firestore-openfort-transactions), which incorporates the [Openfort SDK](https://github.com/openfort-xyz/openfort-node). This enables Unity clients to directly leverage Openfort's blockchain capabilities, allowing for sophisticated blockchain interactions within the gaming environment.

## Application Workflow

//TODO

## Prerequisites
1. ### Clone or download the repository and open it with Unity [2021.3](https://unity.com/releases/editor/qa/lts-releases?version=2021.3).
  When opening the project, select ***Ignore*** on this popup:

  ![Alt text](image.png)

  Once opened, you will see some reference errors. We will solve this in the next step by importing the Firebase SDK.

2. ### Follow the [Firebase-Unity setup guide](https://firebase.google.com/docs/unity/setup?hl=es-419).
  On [step 4](https://firebase.google.com/docs/unity/setup?hl=es-419#add-sdks), you just need to import ***FirebaseAuth*** and ***FirebaseFirestore*** packages:

  ![Alt text](image-1.png)

  Do it one by one and disable ***ExternalDependencyManager*** folder before importing:

  ![Alt text](image-2.png)

  Most reference errors should be solved by now. If `UnityEditor.iOS.Extensions.Xcode` error is still standing, select ***Firebase.Editor*** asset, disable ***Validate References*** and choose ***Apply***:

  ![Alt text](image-3.png)

## Set up Firebase app

### Add Sign-in providers

Go to the [Firebase console](https://console.firebase.google.com/?hl=es-419), select your project and select ***Authentication***:

![Alt text](image-4.png)

Select ***Get started***:

![Alt text](image-5.png)

Select ***Google*** as a sign-in provider:

![Alt text](image-6.png)

Activate ***Enable*** toggle, choose a public-facing name and select ***Save***:

![Alt text](image-7.png)

After choosing ***Done*** on the popup that will appear, you will see your Google provider enabled:

![Alt text](image-8.png)



## Set up Openfort //TODO?

1. #### [Add a Contract](https://dashboard.openfort.xyz/assets/new)
   This sample requires a contract to run. We use [0x38090d1636069c0ff1Af6bc1737Fb996B7f63AC0](https://mumbai.polygonscan.com/address/0x38090d1636069c0ff1Af6bc1737Fb996B7f63AC0) (NFT contract deployed in 80001 Mumbai). You can use this for the guide:

   <img src="docs-img/image-1.png" width="500">

2. #### [Add a Policy](https://dashboard.openfort.xyz/policies/new)
   We aim to cover gas fees for users. Set a new gas policy:

   <img src="docs-img/image.png" width="500">

   Now, add a rule so our contract uses this policy:

   <img src="docs-img/image-2.png" width="500">

## Set up Unity Client

This Unity sample project is already equipped with:
+ //TODO [Firebase]()
+ [Google Play Games Unity Plugin (v11.01)](https://github.com/playgameservices/play-games-plugin-for-unity)

To begin, open [unity-client](https://github.com/openfort-xyz/playfab-unity-sample/tree/main/unity-client) with Unity:

1. #### TODO FIREBASE?

2. #### Configure Google Play Games SDK
    - Even if you've set up the Google Play Games SDK following the [required tutorial](https://www.youtube.com/watch?v=dbLpA2YB6vU), ensure that you've correctly configured all fields by navigating to ***Window --> Google Play Games --> Setup --> Android setup***:

      ![Google Play Games Config 1](docs-img/image-30.png)
      
      <img src="docs-img/image-31.png" width="500">

    By doing this, when the game runs on Android, it will utilize Google Play Games for user authentication via PlayFab. Otherwise, the default PlayFab authentication will be used.

## Test in Editor

## Test on Android

Upon building and running the game on an Android device, the registration/login process is automated via Google Play Games, resulting in a streamlined user experience.

## Conclusion

Upon completing the above steps, your Unity game will be fully integrated with Openfort and Firebase. Always remember to test every feature before deploying to guarantee a flawless player experience.

For a deeper understanding of the underlying processes, check out the [tutorial video](//TODO). 

## Get support
If you found a bug or want to suggest a new [feature/use case/sample], please [file an issue](../../issues).

If you have questions, comments, or need help with code, we're here to help:
- on Twitter at https://twitter.com/openfortxyz
- on Discord: https://discord.com/invite/t7x7hwkJF4
- by email: support+youtube@openfort.xyz