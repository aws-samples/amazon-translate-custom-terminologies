# About

[Amazon Translate](https://aws.amazon.com/translate/) Custom Terminologies
Samples for .NET Core using C#

Amazon Translate is a neural machine translation service that delivers fast,
high-quality, and affordable language translation. Neural machine translation is
a form of language translation automation that uses deep learning models to
deliver more accurate and more natural sounding translation than traditional
statistical and rule-based translation algorithms. Amazon Translate allows you
to localize content - such as websites and applications - for international
users, and to easily translate large volumes of text efficiently.

Using
[custom terminology](https://docs.aws.amazon.com/translate/latest/dg/how-custom-terminology.html)
with your translation requests enables you to make sure that your brand names,
character names, model names, and other unique content is translated exactly the
way you need it, regardless of its context and the Amazon Translate algorithm’s
decision.

It's easy to set up a terminology file and attach it to your Amazon Translate
account. When you translate text, you simply choose to use the custom
terminology as well, and any examples of your source word are translated as you
want them.

# Prerequisites

- [Dotnet Core 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)
- [AWS CLI](https://docs.aws.amazon.com/polly/latest/dg/setup-aws-cli.html) for
  running AWS CLI commands after configuring a
  [default or named profile](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-configure.html)

# Run

After downloading the sample, run the following command from the downloaded
folder

```
dotnet run
```

Result of execution

```
Source: Amazon Translate is a text translation service that uses advanced machine learning technologies to provide high-quality translation on demand.
Translation: अमेज़ॅन अनुवाद एक पाठ अनुवाद सेवा है जो मांग पर उच्च गुणवत्ता वाले अनुवाद प्रदान करने के लिए उन्नत मशीन सीखने की तकनीकों का उपयोग करती है।
```

Result after applying
[custom Terminology](https://docs.aws.amazon.com/translate/latest/dg/how-custom-terminology.html)
that helps retain the keywords "Amazon Translate", and "Machine Learning" with
help of a
[csv terminology file](https://docs.aws.amazon.com/translate/latest/dg/using-ct.html)

```
After applying custom terminology:
Preserve_Amazon_Translate
Translation: Amazon Translate एक पाठ अनुवाद सेवा है जो मांग पर उच्च गुणवत्ता वाले अनुवाद प्रदान करने के लिए उन्नत Machine Learning की तकनीकों का उपयोग करती है।
```

# Dependencies

appsettings.json file uses your default AWS profile so that you don't have to
set AWS credentials in clear text

```
{
  "AWS": {
    "Profile": "default",
    "Region": "us-west-2"
  }
}
```

### A quick walkthrough of the .csproj file

.csproj file: required .NET libraries - these libraries will be auto-installed
as part of the build process

```
<ItemGroup>
    <PackageReference Include="AWSSDK.Translate" Version="3.3.100.32" />
    <PackageReference Include="AWSSDK.Extensions.NETCore.Setup" Version="3.3.100.1" />
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="2.2.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="2.2.0" />
</ItemGroup>

<ItemGroup>
    <None Update="appsettings.json">
        <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
    <None Update="custom-terminology.csv">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
</ItemGroup>
```
