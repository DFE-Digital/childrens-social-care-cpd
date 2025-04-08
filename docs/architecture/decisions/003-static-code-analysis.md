# Static Code Analysis

## Status - Proposed

## Context

Our software development lifecycle currently leverages SonarCloud for static code analysis, vulnerability detection, and code quality assessment. While SonarCloud provides valuable insights, we are facing challenges related to integration with our existing GitHub-centric workflow, complex administration and maintenance and the need for comprehensive security scanning across our entire software supply chain.

## Decision

We will transition from SonarCloud to a combination of GitHub Advanced Security (GHAS) and Trivy for our static code analysis, vulnerability scanning, and software composition analysis needs.

## Rationale

- Seamless GitHub integration:
  - GHAS has natively integrated with GitHub, our primary code repository and CI/CD platform. This eliminates the need for complex integrations and reduces friction in the developer workflow.
  - Security alerts and findings are directly visible within GitHub, allowing developers to address issues within their familiar environment.
- Cost Optimisation
  - GHAS is included with GitHub Enterprise licenses, potentially reducing our overall security tooling costs compared to a separate SonarCloud subscription.
  - Trivy is Open Source and free.
- Comprehensive Security Coverage
  - GHAS provides a suite of security features, including:
    - Code scanning (Static Analysis)
    - Secret scanning
    - Dependency review
  - Trivy excels at software composition scanning (SCA) and container vulnerability scanning, ensuring we have comprehensive coverage across our application dependencies and containerised deployments.
  - This combination allows for more complete coverage than SonarCloud alone, especially regarding infrastructure as code, and containerized applications.
- Improved developer experience
  - Direct integration with pull requests and CI/CD pipelines allows for immediate feedback on code quality and security issues.
  - Developers can address issues directly within GitHub, reducing context switching and improving efficiency.
- Enhanced software supply chain security
  - Trivy's ability to scan container images and file systems provides valuable insights into the security of our software supply chain.
  - GHAS dependency review will also provide better supply chain security.
- Flexibility and customisation
  - GitHub Actions allows for greater flexibility in customizing our security scanning workflows.
  - Trivy is a very flexible tool, that is easily configured, and can be used in many places in the pipeline.

## References
