# Static Error Pages

This application uses two static HTML error pages with DFE branding style. These pages are used by the App gateway to show error
messages when service is down or App gateway not able to reach the web application.

Repository path - https://github.com/DFE-Digital/childrens-social-care-cpd/tree/master/Childrens-Social-Care-CPD/Views/HTMLFiles

File Names:

1. 403.HTML
2. 503.HTML

These pages are created using the following steps:

1. Create empty template for HTML file
2. Add required inline CSS in HTML template. Do not add links for the external CSS sources. CSS needs to be added inline only.
3. Add required JS script inline. Do not add links for the external Js file sources.
4. Add required HTML for DFE branding and contents.
5. Create Base64 string for any images, icons needs to be used withing the HTML file.
6. Use this Base64 string for images used for icons or Image Urls used in the CSS classes.

for e.g. 

 .govuk-footer__copyright-logo {
            display: inline-block;
            min-width: 125px;
            padding-top: 112px;
            background-image: url('data:image/png;base64,<Base64 image string>)
            background-position: 50% 0%;
            background-size: 125px 102px;
            text-align: center;
            white-space: nowrap;
        }


7. Test this HTML file using the browser.

Deployment:

These static HTML error pages are deployed in Azure blob storage. App gateway use these pages to render it as per the rules set.
Web pages can be uploaded or removed using Azure storage browser UI interface.

Follow these steps to deploy HTMl files

1. Log in to Azure
2. Go to the Storage Browser Azure service
3. Go to the storage account - s185errorpage

![Storage](./images/storage.png)

4. Go to the blob container - s185errorpage

![Container](./images/container.png)

5. Files can be uploaded, deleted using the blob container explorer

![ContainerExplorer](./images/containerExplorer.png)



