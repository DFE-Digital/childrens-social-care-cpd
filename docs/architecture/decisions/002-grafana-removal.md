# Grafana Removal

## Statis - Accepted

## Context
The team discussed the reporting for KPIs and service statistics that current use Grafana. This allowed a single dash board for all reporting from the system and site.

The current system is using Grafana and has a daily failure that does not effect users of the system. However the failure will report and restart for 5 minutes at approximately 1:00am for 5 minutes. As the usage of the site has increased this failure can now happen outside the original time window.

The BA on the project created a report of what data was required for the KPI reporting and concluded that only a single figure from the Grafana dashboard was used which could not be traced to an external data source.

## Decision
Due to the lack of use-case of the Grafana, the cost of running, and the inherent issues with its stability, Grafana has been schedule for removal from the system.



## References

SharePoint: Review of Reporting & Grafana Tool.doc

https://educationgovuk.sharepoint.com/:w:/r/sites/Vulnerablechildrenandfamiliesportfolio/Shared%20Documents/Childrens%20social%20care/9.%20CSC%20Career%20Progression%20(Develop%20your%20career%20in%20Child%20%26%20family%20social%20work%20service)/08%20Beta/04%20Business%20Analysis/Documentation/Analytical%20Tools/Review%20of%20Reporting%20%26%20Grafana%20Tool.docx?d=w2206ff3b93d24261833c95721e131bbc&csf=1&web=1&e=epV9Ty
