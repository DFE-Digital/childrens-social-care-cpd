# Azure Application Gateway and Front Door

## Status - Proposed

## Context

We are currently using Azure Application Gateway to manage incoming traffic to our web application. We are evaluating the potential benefits of switching to Azure Front Door. However, we have tight timescales for the platform enhancement work and introducing a new service like Front Door carries additional risk.

We only require regional availability and do not currently utilize a CDN. Furthermore, the organisation considers a public IP address as a security risk, which is a requirement for Application Gateway.

## Decision

We will continue to use Azure Application Gateway for the following reasons:

- _Familiarity and Existing Configuration:_ The team is already familiar with Application Gateway and has it configured to meet our current needs.
- _Reduced Risk:_ Introducing Front Door would require reconfiguration and testing, increasing the risk of delays or introducing new issues within our tight project timeline.
- _Sufficient Functionality:_ Application Gateway currently provides all the features we require, including:
  - Layer 7 load balancing
  - SSL termination
  - Web Application Firewall
  - URL-based routing
- _Regional Focus:_ Since we only require regional availability, the global capabilities of Front Door are not a significant advantage in our current situation.
- _No CDN Requirement:_ As we are not currently using a CDN, Front Door's CDN integration does not provide a compelling benefit.
- _Mitigation of Public IP Risk:_ While Application Gateway requires a public IP, we will mitigate the associated risk by implementing the following security measures:
  - _Network Security Groups (NSGs):_ Configure NSGs to restrict inbound and outbound traffic to only necessary ports and IP addresses.
  - _Web Application Firewall (WAF):_ Utilize the built-in WAF in Application Gateway to protect against common web exploits and vulnerabilities.
  - _Regular Security Assessments:_ Conduct security assessments and penetration testing to identify and address any potential vulnerabilities.

## Consequences

- _Potential Benefits of Front Door Not Realised:_ While we don't need global availability or CDN now, if these needs arise in the future, migrating to Front Door later might be more complex.
- _Residual Public IP Risk:_ Despite mitigation measures, some risk associated with the public IP address may remain.

## Alternatives Considered

- _Migrate to Azure Front Door:_ This would eliminate the public IP requirement but introduce significant risk and complexity within our current tight timescales and limited requirements.

## Rationale

Given the tight timescales, the sufficient functionality of Application Gateway for our current regional needs, the mitigation measures in place for the public IP risk, and the fact that we don't require global availability or CDN features, the reduced risk and maintained momentum of sticking with Application Gateway outweigh the potential benefits of Front Door at this time.
