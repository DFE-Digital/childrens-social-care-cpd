import contentful from 'contentful';
import chalk from 'chalk';
import core from '@actions/core';
import axios from 'axios';
import * as cheerio from 'cheerio';
import * as xmlbuilder2 from 'xmlbuilder2';
import * as fs from 'node:fs';

const red = chalk.bold.red;
const spaceId = process.env.SPACE_ID;
const contentfulEnvironment = process.env.CONTENTFUL_ENVIRONMENT;
const deliveryKey = process.env.DELIVERY_KEY;
const sitemapFilePath = process.env.SITEMAP_FILE_PATH;
let websiteRoot = process.env.WEBSITE_ROOT;
let scannedPages = [];

try {
    if (!deliveryKey) throw new Error("Environment variable DELIVERY_KEY not set");
    if (!spaceId) throw new Error("Environment variable SPACE_ID not set");
    if (!contentfulEnvironment) throw new Error("Environment variable CONTENTFUL_ENVIRONMENT not set");
    if (!websiteRoot) throw new Error("Environment variable WEBSITE_ROOT not set");
    if (!sitemapFilePath) throw new Error("Environment variable SITEMAP_FILE_PATH not set");
}
catch (e) {
    core.setFailed(red(e));
    process.exit();
}

const contentfulClient = contentful.createClient({
    accessToken: deliveryKey,
    space: spaceId,
    environment: contentfulEnvironment
});

const setup = () => {

    // trim trailing / from website root if exists
    if (websiteRoot.slice(-1) == '/') {
        websiteRoot = websiteRoot.slice(0, -1)
    }
}

const fetchAllEntries = async function  () {
    let finished = false,
    skip = 0;
    let entries = [];

    try {
        while (!finished) {
            let response = await contentfulClient.getEntries({
                limit: 500,
                skip: skip
            });

            for (let item of response.items) {
                entries.push(item);
            }

            if (response.items.length == 0) {
                finished = true;
            }
            skip += response.items.length;
        }
        return entries;
    }
    catch (e) {
        core.setFailed(red(e));
        process.exit();
    }
}

const filterContentEntries = (items) => {
    let filteredItems = [];
    for (let item of items) {
        if (item.sys.contentType.sys.id == "content") {
            filteredItems.push({
                id: websiteRoot + '/' + item.fields.id,
                lastUpdated : item.sys.updatedAt
            });
        }
    }
    return filteredItems;
}

const transformSitemapUrl = (url) => {

    if (url.startsWith('/')) {
        url = websiteRoot + url;
    }

    if (url.slice(-1) == '/') {
        url = url.slice(0, -1)
    }

    if (url.indexOf('?') > -1) {
        url = url.slice(0, url.indexOf('?'));
    }
    return url;
}

const validPageForSitemap = (url) => {

    if (url.startsWith('//assets.ctfassets.net')) return false;
    if (url.startsWith('/') || url.startsWith(websiteRoot)) {
        return transformSitemapUrl(url);
    }
    return false;
}

const scanPage = async (url, foundPages=[], level=0) => {

    try {
        // request the target website
        const response = await axios.get(url);
        const $ = cheerio.load(response.data);
        const links = $('a[href]');

        let childPagesToScan = [];

        for (const element of links) {
            let linkHref = validPageForSitemap($(element).attr('href'));

            if (linkHref) {
                if (scannedPages.indexOf(linkHref) == -1) {
                    scannedPages.push(linkHref);
                    childPagesToScan.push(linkHref);
                }
            }
        }

        let priority;
        switch (level) {
            case 0: priority = '1.0'; break;
            case 1:
            case 2: priority = '0.8'; break;
            case 3:
            case 4: priority = '0.7'; break;
            case 5: priority = '0.6'; break;
            default: priority = '0.5'
        }

        for (const childPageUrl of childPagesToScan) {
            foundPages.push({
                url: childPageUrl,
                priority: priority
            });
            let childPages = await scanPage(childPageUrl, foundPages, level+1);
            foundPages.concat(childPages);
        }

    } catch (error) {
        // handle any error that occurs during the HTTP request
        console.error(`Error fetching ${url}: ${error.message}`);
    }

    return foundPages;
}

const buildSitemap = (pages, pageMetadata) => {

    let xmlDoc = xmlbuilder2.create({ version: '1.0'})
        .ele('urlset', {xmlns: 'https://www.sitemaps.org/schemas/sitemap/0.9'});
    
    for (let page of pages) {
        let metadata = pageMetadata.find(m => m.id == page.url);
        if (metadata) {
            xmlDoc
                .ele('url')
                .ele('loc').txt(page.url).up()
                .ele('lastmod').txt(metadata.lastUpdated).up()
                .ele('priority').txt(page.priority).up()
                .up();
        }
    }

    return xmlDoc.end({ prettyPrint: true });
}

const writeSitemapFile = (sitemapContents) => {
    try {
        fs.writeFileSync (sitemapFilePath, sitemapContents, 'utf8');
    }
    catch (e) {
        core.setFailed(red(e));
        process.exit();
    }
}

setup();

let allContentfulEntries = await fetchAllEntries();

let contentfulPageDefinitions = filterContentEntries(allContentfulEntries);

let websitePages = await scanPage(websiteRoot);

let sitemap = buildSitemap (websitePages, contentfulPageDefinitions);

writeSitemapFile(sitemap);

