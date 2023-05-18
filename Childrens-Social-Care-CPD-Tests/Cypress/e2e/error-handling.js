import { Given, When, Then } from "@badeball/cypress-cucumber-preprocessor";

Given("a user has arrived on the home page", () => {
    cy.visitcpdhomepage();
});

When("a service page has not been found", () => {
    cy.visit("/Error/Error/404", { failOnStatusCode: false });
});

When("a the service is down", () => {
    cy.visit("/Error/Error/500", { failOnStatusCode: false });
});

Then("the {string} error page is displayed", (title) => {
    cy.get("h1").should("contain.text", title);
});