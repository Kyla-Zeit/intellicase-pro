# IntelliCase Pro Android

Native Android Studio prototype for IntelliCase Pro, built with Kotlin and Jetpack Compose.

This is not a WebView wrapper. It is a real Android app project with a mobile-native login, dashboard, case list, evidence register, calendar, billing view, and reports view using the same demo product data as the ASP.NET Core web app.

## Quick demo APK

For the easiest test install, download the demo APK from the repo root:

[Download IntelliCasePro-Android-demo.apk](https://github.com/Kyla-Zeit/intellicase-pro/raw/main/downloads/IntelliCasePro-Android-demo.apk)

Android may ask you to allow installs from your browser or file manager. This APK is debug-signed for demo/review installs and is not meant for Play Store distribution.

## Open in Android Studio

1. Open Android Studio.
2. Choose **File > Open**.
3. Select this folder:

   ```text
   android
   ```

4. Let Gradle sync.
5. Run the `app` configuration on an Android emulator or device.

If Android Studio asks to install Android SDK Platform 35 or build tools, accept that prompt.

## Build from the terminal

You can build the Android app without opening Android Studio.

Prerequisites:

- JDK 17 or newer
- Android SDK Platform 35
- Android SDK Build Tools
- Android platform tools if you want to install with `adb`

From the repository root:

```powershell
cd android
.\gradlew.bat :app:assembleDebug
```

The debug APK is created at:

```text
android/app/build/outputs/apk/debug/app-debug.apk
```

## Open without Android Studio

If you only want to try the app, install the built APK on an Android phone or emulator.

You can use the included demo APK in `downloads/`, build the APK locally with Gradle, or download an APK from the repo's GitHub Releases if a demo APK has been attached there.

Install with Android platform tools:

```powershell
adb install -r ../downloads/IntelliCasePro-Android-demo.apk
```

You can also transfer the APK to an Android phone and open it there. The phone may ask you to allow installs from that file source.

The debug APK is automatically signed for testing. It is meant for demo/review installs, not Play Store distribution.

## Demo login

The first prototype accepts the seeded demo credentials:

```text
jane@intellicasepro.local
Demo#2026!
```

## What is included

- Kotlin + Jetpack Compose Android project
- Material 3 mobile UI
- Native login screen
- Dashboard metrics and active case cards
- Cases, evidence, calendar, billing, and reports sections
- Local demo data ported from the original IntelliCase Pro seed data

## Next build step

The Android app currently uses local demo data. The next practical step is choosing whether it should:

- connect to the existing ASP.NET Core backend APIs,
- use a local Room database for offline-first field work,
- or do both with sync.
