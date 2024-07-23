 if (typeof __JSWRAPPER === 'undefined' || (!__JSWRAPPER.initialized())) {
     if ((typeof ServiceWorkerContainer !== 'undefined' && globalThis instanceof ServiceWorkerContainer) || (typeof WorkerGlobalScope !== 'undefined' && globalThis instanceof WorkerGlobalScope)) {
         importScripts('https://mcas-proxyweb.mcas.ms/js-bootstrap.js?saasId=-1&type=WORKER&McasTsid=-1&origin=' + encodeURIComponent(globalThis.origin));
     } else {
         /* eslint-disable */
         eval(function () {
             const x = new XMLHttpRequest();
             x.open('GET', 'https://mcas-proxyweb.mcas.ms/js-bootstrap.js?saasId=-1&type=WINDOW&McasTsid=-1&origin=' + encodeURIComponent(globalThis.origin), false);
             x.withCredentials = true;
             x.send();
             return x.responseText;
         }());
         /* eslint-enable */
     }
};
/* module-key = 'com.atlassian.jira.jira-atlaskit-plugin:global-cache', location = 'jira-atlaskit-module/js/cache-api.js' */
!(function(){var n,e=new Promise(function(e){n=e;});AJS.namespace("JIRA.API"),JIRA.API.getCache=function(){return e;},define("jira/cache",function(){return function(e){n(e);};});}());;