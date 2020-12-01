# Certificate Generator

Create Apple certificate from anywhere, not just macOS.

This tool allows the creation of certificate signing requests compatible with Apple Developer console. Once the certificate has been created, the tool also creates a `.p12` file for safekeeping.

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

## Getting Started
This program creates a certificate signing request to be used in the Apple Developer console to obtain a certificate. A P12 is then generated using this certificate. Supported certificate types are push notifications and signing certificate.

From `--help`:
```
Usage:
  Certificate.Generator [options]

Options:
  --email-address <email-address>            A relevant email address to use in the request (ex: the mail of the owner
                                             of the account)
  --common-name <common-name>                The name to use in the request; usually the name of the Apple Developer
                                             organization
  --country-code <country-code>              The country code of the account
  --request-file-path <request-file-path>    Optional: the path where to place the .csr file
  --p12-file-path <p12-file-path>            Optional: the path where to place the .p12 file
  --version                                  Display version information
```

## Features

- Creation of `.csr` (or `.certSigningRequest`) files compatible with the Apple Developer console (Push, Distribution, Development, etc.)
- Creation of the `.p12` file corresponding to the `.cer` delivered by Apple.

## Changelog

Please consult the [CHANGELOG](CHANGELOG.md) for more information about version
history.

## License

This project is licensed under the Apache 2.0 license - see the
[LICENSE](LICENSE) file for details.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on the process for
contributing to this project.

Be mindful of our [Code of Conduct](CODE_OF_CONDUCT.md).

## Acknowledgments
