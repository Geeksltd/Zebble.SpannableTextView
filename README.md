[logo]: https://raw.githubusercontent.com/Geeksltd/Zebble.SpannableTextView/master/icon.png "Zebble.SpannableTextView"


## Zebble.SpannableTextView

![logo]

A Zebble plugin to set different style to a TextView in Zebble applications.

[![NuGet](https://img.shields.io/nuget/v/Zebble.SpannableTextView.svg?label=NuGet)](https://www.nuget.org/packages/Zebble.SpannableTextView/)

> SpannableTextView is a TextView for Zebble application which make you able to add different style to a TextView like bold, italic, fontColor and fontSize by adding some style tags into your text. This plugin implemented for Android, IOS and UWP Zebble applications and it is available on NuGet.

<br>


### Setup
* Available on NuGet: [https://www.nuget.org/packages/Zebble.SpannableTextView/](https://www.nuget.org/packages/Zebble.SpannableTextView/)
* Install in your platform client projects.
* Available for iOS, Android and UWP.
<br>


### Api Usage

To use SpannableTextView into your Zebble application you need to install it from NuGet for all of platforms that you need. After that, you need to add this element to your Zebble page or code behind and set the SpannableText property of it like the code which mentioned below:
```csharp
MySpannableTextView.SpannableText = "<b>Lorem</b> <i>ipsum</i> <font color='#ffffff'>dolor<font color='red'>sit</font> " +
"<font color='blue'><i>amet</i></font></font>, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." +
" Ut <font size='50'>enim</font> ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo " +
"consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat " +
"cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est <b><i>laborum</i></b>.";
```
#### Supported HTML tags

* ``<b>``
* ``<bold>``
* ``<i>``
* ``<italic>``
* ``<font size=�� color=�� >``

#### Examples

For using in Zebble pages:
```xml
<z-Component z-type="SamplePage" z-base="Templates.Default" z-namespace="UI.Pages"
     data-TopMenu="MainMenu" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    xsi:noNamespaceSchemaLocation="./../.zebble-schema.xml" >

  <z-place inside="Body">

    <SpannableTextView Id="MySpannableTextView" />
    
  </z-place>
  
</z-Component>
```
And in code behind of your page:

```csharp
public async override Task OnInitializing()
{
    await base.OnInitializing();

    MySpannableTextView.SpannableText = "<b>This</b> <i><font color='red'>is</font></i> a <font size='50' color='green'>sample <bold>text</bold></font>.";
}
```
<br>


### Properties
| Property     | Type         | Android | iOS | Windows |
| :----------- | :----------- | :------ | :-- | :------ |
| SpannableText           | string          | x       | x   | x       |



<br>


### Events
| Event             | Type                                          | Android | iOS | Windows |
| :-----------      | :-----------                                  | :------ | :-- | :------ |
| SpannableTextChanged            | AsyncEvent    | x       | x   | x       |
