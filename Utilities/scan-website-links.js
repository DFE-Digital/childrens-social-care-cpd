import chalk from 'chalk';
import core from '@actions/core';
import axios from 'axios';
import * as cheerio from 'cheerio';

const red = chalk.bold.red;

let websiteRoot = process.env.WEBSITE_ROOT;
let scannedPages = [],
    externalLinks = [],
    brokenLinks = {
        internal: [],
        external: []
    };


if (!websiteRoot) {
    core.setFailed(red('Environment variable WEBSITE_ROOT not set'));
    process.exit();
}

const setup = () => {

    // trim trailing / from website root if exists
    if (websiteRoot.slice(-1) == '/') {
        websiteRoot = websiteRoot.slice(0, -1)
    }
};

const transformInternalUrl = (url) => {

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
};

const internalPageToScan = (url) => {

    if (url.startsWith('//assets.ctfassets.net')) return false;
    if (url.startsWith('/') || url.startsWith(websiteRoot)) {
        return transformInternalUrl(url);
    }
    return false;
};

const ignoreUrl = (url) => {
    if (url.startsWith('//assets.ctfassets.net')) return true;
    if (url.startsWith('#')) return true;
    if (url.startsWith('mailto:')) return true;
    let ignoreRelativeUrls = [ 'home', 'contents' ];
    if (ignoreRelativeUrls.includes(url)) return true;
    return false;
};

const addBrokenLink = (url, pages, type) => {
    if (!Array.isArray(pages)) {
        pages = [pages];
    }
    console.log('adding', url, pages, type);
    
    let idx = brokenLinks[type].findIndex(x => x.url === url);
    if (idx === -1) {
        brokenLinks[type].push({
            url: url,
            pages: pages
        });
    }
    else {
        for (let page of pages) {
            brokenLinks[type][idx].pages.push(page);
        }
    }
};

const scanPage = async (url, parent='') => {

    try {
        // request the target website
        const response = await axios.get(url);
        const $ = cheerio.load(response.data);
        const links = $('a[href]');

        let childPagesToScan = [];

        for (const element of links) {
            let linkHref = internalPageToScan($(element).attr('href'));

            if (linkHref) {
                if (scannedPages.indexOf(linkHref) == -1) {
                    scannedPages.push(linkHref);
                    childPagesToScan.push(linkHref);
                }
            }
            else {
                let externalLink = $(element).attr('href');

                if (!ignoreUrl(externalLink)) {
                    let idx = externalLinks.findIndex(x => x.url == externalLink);

                    if (idx > -1) {
                        externalLinks[idx].pages.push(url);
                    }
                    else {
                        externalLinks.push({
                            url: externalLink,
                            pages: [
                                url
                            ]
                        });
                    }
                }
            }
        }

        for (const childPageUrl of childPagesToScan) {
            await scanPage(childPageUrl, url);
        }

    } catch (error) {
        // handle any error that occurs during the HTTP request
        console.error(`Error fetching ${url}: ${error.message}`, 'parent', parent);
        addBrokenLink(url, parent, 'internal');
    }
};

const delay = ms => new Promise(res => setTimeout(res, ms));

const checkLink = async (link) => {
    var responseStatus;
    try {
        // wait five second - we don't want to flood servers with loads of requests
        await delay (5000);
    
        const response = await axios.get(link, {
            headers: {
              "Accept": "*/*, application/json, text/plain",
              "Referer": "https://www.support-for-social-workers.education.gov.uk",
              "Referrer-Policy": "strict-origin-when-cross-origin"
            }});

        responseStatus = response.status;
    }
    catch (err) {
        if (err.status) {
            responseStatus = err.status;
        }
    }
    console.log('status', responseStatus);
    return responseStatus;
};

const checkLinks = async (externalLinks) => {

    externalLinks.sort((a,b) => {
        if (a.url < b.url) return -1;
        return 1;
    });

    for (let count=0; count < externalLinks.length; count ++) {
        let externalLink = externalLinks[count];
        let status = await checkLink(externalLink.url);
        if (status === 404) {
            addBrokenLink(externalLink.url, externalLink.pages, 'external');
        }
        if (count == 9) return;
    }
};

const reportOutput = () => {
    console.log(brokenLinks);
};

setup();
await scanPage(websiteRoot);
await checkLinks(externalLinks);
reportOutput();
