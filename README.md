## Overview

[Firebase](https://firebase.google.com/) is Google's all-encompassing app development platform, providing game developers with an array of backend tools powered by Google Cloud.

Within Unity, we've integrated the Firebase SDK to facilitate backend functionalities. Coupled with the [Google Play Games plugin for Unity](https://github.com/playgameservices/play-games-plugin-for-unity), it ensures a secure and efficient login for Android users.

The integration is further enhanced with the [Openfort Firebase extension](https://extensions.dev/extensions/openfort/firestore-openfort-transactions), which incorporates the [Openfort SDK](https://github.com/openfort-xyz/openfort-node). This enables Unity clients to directly leverage Openfort's blockchain capabilities, allowing for sophisticated blockchain interactions within the gaming environment.

## Application Workflow

//TODO

## Prerequisites

- ### Sign in to [dashboard.openfort.xyz](http://dashboard.openfort.xyz) and create a new project.
- ### You need a [Google Play Developer account](https://support.google.com/googleplay/android-developer/answer/6112435?hl=en).
- ### You need a [Google Cloud project](https://developers.google.com/workspace/guides/create-project).
- ### Clone or download the repository and open it with Unity [2021.3](https://unity.com/releases/editor/qa/lts-releases?version=2021.3).
  When opening the project, select ***Ignore*** on this popup:

  ![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_03a708cc91.png?updated_at=2023-11-07T18:41:24.181Z)

  Once opened, you will see some reference errors. We will solve this in the next step by importing the Firebase SDK.
- ### Follow the [Firebase-Unity setup guide](https://firebase.google.com/docs/unity/setup?hl=es-419).
  On [step 4](https://firebase.google.com/docs/unity/setup?hl=es-419#add-sdks), you just need to import ***FirebaseAuth*** and ***FirebaseFirestore*** packages:

  ![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_1_7d8a33fb8d.png?updated_at=2023-11-07T18:41:24.676Z)

  Do it one by one and disable ***ExternalDependencyManager*** folder before importing:

  ![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_2_352fc42325.png?updated_at=2023-11-07T18:41:37.181Z)

  Most reference errors should be solved by now. If `UnityEditor.iOS.Extensions.Xcode` error is still standing, select ***Firebase.Editor*** asset, disable ***Validate References*** and choose ***Apply***:

  ![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_3_9ea2133564.png?updated_at=2023-11-07T18:41:46.887Z)
- ### Create a keystore
  Follow this [guide](https://docs.unity3d.com/Manual/android-keystore-create.html) to create a new keystore for the Unity project.
- ### Find SHA1 certificate fingerprint
  You need to extract the certificate fingerprint from the created keystore. Follow this [video tutorial](https://www.youtube.com/watch?v=lDXE4lfM0aQ) on how to do it, it also covers the creation of the keystore.

## Set up Firebase

### Add Google sign-in provider

Go to the [Firebase console](https://console.firebase.google.com/?hl=es-419), select your project and select ***Authentication***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_4_e8b0d867f8.png?updated_at=2023-11-07T18:41:47.295Z)

Select ***Get started***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_5_140cfd6f28.png?updated_at=2023-11-07T18:41:35.185Z)

Select ***Google*** as a sign-in provider:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_6_eacbe66a92.png?updated_at=2023-11-07T18:41:38.487Z)

Activate ***Enable*** toggle, choose a public-facing name and select ***Save***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_7_4b75cbc33e.png?updated_at=2023-11-07T18:41:40.678Z)

A popup will appear. Copy the ***Web client ID*** and the ***Web client secret*** somewhere safe and choose ***Done***. You will see your Google provider enabled:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_8_7d2b07af1c.png?updated_at=2023-11-07T18:41:29.178Z)

Select the provider and choose ***Project Settings***. Under ***Your apps*** section select ***Add fingerprint*** and add your [SHA1 certificate fingerprint](https://github.com/openfort-xyz/firebase-extension-unity-sample/tree/main#find-sha1-certificate-fingerprint). Then choose ***Save***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_26_0877bd3b91.png?updated_at=2023-11-07T18:41:44.183Z)

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_27_9505073e7c.png?updated_at=2023-11-07T18:41:29.584Z)

### Add Google Play sign-in provider

Select ***Add new provider*** and choose ***Google Play***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_9_7c803a1a83.png?updated_at=2023-11-07T18:41:36.279Z)

Activate ***Enable*** toggle, enter the credentials you just saved and choose ***Save***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_10_3cd041e888.png?updated_at=2023-11-07T18:41:33.579Z)

Both ***Google*** and ***Google Play*** sign-in providers are ready:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_11_0a946e50ff.png?updated_at=2023-11-07T18:41:31.578Z)

### Install Openfort Extension

Go to the [Firebase Extensions Hub](https://extensions.dev/extensions/openfort/firestore-openfort-transactions) and choose ***Install in Firebase console***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_37_663d8ce948.png?updated_at=2023-11-07T18:41:43.778Z)

Choose your project to continue:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_38_129ce9d653.png?updated_at=2023-11-07T18:41:44.982Z) 

Set up your billing profile and follow the instructions until you need to insert the [Openfort API Secret key](https://dashboard.openfort.xyz/apikeys) and choose ***Create secret***. Leave all the other settings as default:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_39_b21cd55281.png?updated_at=2023-11-07T18:41:33.281Z)

Finally choose ***Install extension***. After 3-5 minutes you will see the extension installed:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_41_9c8adab805.png?updated_at=2023-11-07T18:41:46.977Z)

Now select ***Get started*** and under ***How this extension works*** section find ***Configure Openfort webhooks***. Copy the URL:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_42_c9e5cfa74b.png?updated_at=2023-11-07T18:41:46.182Z)

Go to the [Openfort dashboard - Webhooks](https://dashboard.openfort.xyz/webhooks) and choose ***Add webhook***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_40_47d93997bf.png?updated_at=2023-11-07T18:41:38.486Z)

Paste the webhook URL and leave the *Type* as it is. Choose ***Add webhook***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_44_f0baf395ee.png?updated_at=2023-11-07T18:41:44.581Z)

## Set up Google Play

> **Reminder:** Use the same Google account you used for setting up your Firebase app.

### Create a new app
Go to [Play Console](https://play.google.com/console) and create a new app. Enter app details (it's important you select ***Game***), confirm policies and select ***Create app***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_12_0487318190.png?updated_at=2023-11-07T18:41:44.580Z)

Under ***Grow --> Play Games Services --> Setup and management --> Configuration***, select ***Create new Play Games Services project*** and choose your Firebase project as the cloud project. Then select ***Use***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_13_5ad11592ff.png?updated_at=2023-11-07T18:41:47.787Z)

### Add credentials

#### Add Android OAuth client credential

Under ***Credentials*** section choose ***Add credential***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_14_7e4b6cb5d1.png?updated_at=2023-11-07T18:41:46.982Z)

Select ***Android***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_15_a46a653f09.png?updated_at=2023-11-07T18:41:46.784Z)

Scroll down and select ***Create OAuth client***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_16_27ab5da789.png?updated_at=2023-11-07T18:41:33.186Z)

Choose ***Create OAuth Client ID***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_17_c6d544e6c4.png?updated_at=2023-11-07T18:41:31.682Z)

This will open the Google Cloud console. Now select ***Android*** as *Application type*, enter a *Name* and fill the *Package name* with the **Unity app package name** (found in the Android Platform Player Settings):

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_19_72b69ccb8c.png?updated_at=2023-11-07T18:41:46.678Z)

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_20_e3286e880c.png?updated_at=2023-11-07T18:41:47.076Z)

Enter your [SHA1 certificate fingerprint](https://github.com/openfort-xyz/firebase-extension-unity-sample/tree/main#find-sha1-certificate-fingerprint) and choose ***CREATE***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_21_b1c2c44246.png?updated_at=2023-11-07T18:41:34.781Z)

Now you can download the JSON and choose ***OK***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_22_ee531ceeba.png?updated_at=2023-11-07T18:41:36.781Z)

Go back to the Google Play console, select ***Done*** and choose your newly created Android OAuth client. Then select ***Save changes***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_23_5ff3dc73de.png?updated_at=2023-11-07T18:41:44.877Z)

#### Add Game server/Web OAuth client credential

Go back to ***Configuration*** and select ***Add credential***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_24_b44fc158b9.png?updated_at=2023-11-07T18:41:44.287Z)

Choose ***Game server***, refresh OAuth clients, select ***Web client (auto created by Google Service)*** (it was created automatically during [this process](https://github.com/openfort-xyz/firebase-extension-unity-sample/tree/main#add-google-sign-in-provider)) and select ***Save changes***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_25_1f76c7f29a.png?updated_at=2023-11-07T18:41:45.679Z)

Finally copy the ***OAuth client ID***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_28_ecba674180.png?updated_at=2023-11-07T18:41:30.082Z)

## Set up Unity project

> **Reminder:** Make sure ***Android*** is selected as a platform in ***Build settings***. 

Go to ***Window --> Google Play Games --> Setup --> Android setup***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_30_818b8cbf6d.png?updated_at=2023-11-07T18:41:35.884Z)

Paste the ***Game server OAuth client ID*** you just copied under ***Client ID***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_31_421e75f3b2.png?updated_at=2023-11-07T18:41:32.077Z)

Go to the [Google Play console](https://play.google.com/console) and on your app's configuration select ***Get resources***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_32_52815075fc.png?updated_at=2023-11-07T18:41:44.377Z)

Copy the Android (XML):

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_33_6690691229.png?updated_at=2023-11-07T18:41:38.876Z)

In Unity, paste it in ***Resources Definition*** and then select ***Setup***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_35_b08e422040.png?updated_at=2023-11-07T18:41:46.888Z)

Finally, go to the [Firebase console](https://console.firebase.google.com/?hl=es-419) and under your app configuration, download the ***google-services.json***:

![Alt text](https://strapi-oube.onrender.com/uploads/firebase_extension_unity_img_36_5048e220db.png?updated_at=2023-11-07T18:41:46.980Z)

Import it in your Unity project ***Assets*** folder to make sure every credential is up to date.

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