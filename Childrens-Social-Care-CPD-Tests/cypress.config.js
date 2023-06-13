const { defineConfig } = require("cypress");
const webpack = require("@cypress/webpack-preprocessor");
const preprocessor = require("@badeball/cypress-cucumber-preprocessor");



async function setupNodeEvents(on, config) {

    on('task', {
        log(message) {
            console.log(message)

            return null
        },
        table(message) {
            console.table(message)

            return null
        }

    });

    await preprocessor.addCucumberPreprocessorPlugin(on, config);

    on(
        "file:preprocessor",
        webpack({
            webpackOptions: {
                resolve: {
                    extensions: [".ts", ".js"],
                },
                module: {
                    rules: [
                        {
                            test: /\.ts$/,
                            exclude: [/node_modules/],
                            use: [
                                {
                                    loader: "ts-loader",
                                },
                            ],
                        },
                        {
                            test: /\.feature$/,
                            use: [
                                {
                                    loader: "@badeball/cypress-cucumber-preprocessor/webpack",
                                    options: config,
                                },
                            ],
                        },
                    ],
                },
            },
        })
    );

    // Make sure to return the config object as it might have been modified by the plugin.
    return config;
}

module.exports = defineConfig({
    chromeWebSecurity: false,
    e2e: {
        baseUrl: 'https://www.develop-child-family-social-work-career.education.gov.uk/',
        specPattern: "**/*.feature",
        setupNodeEvents,
    },
    reporter: 'cypress-multi-reporters',
    reporterOptions: {
        reporterEnabled: 'cypress-mochawesome-reporter, mocha-junit-reporter',
        cypressMochawesomeReporterReporterOptions: {
            reportDir: 'cypress/reports',
            charts: true,
            reportPageTitle: 'My Test Suite',
            embeddedScreenshots: true,
            inlineAssets: true
        },
        mochaJunitReporterReporterOptions: {
            mochaFile: 'cypress/reports/junit/results-[hash].xml'
        }
    },
    video: false

});

