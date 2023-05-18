import { Given, Then, When } from "@badeball/cypress-cucumber-preprocessor";

Given("a user has arrived on the home page", () => {
    cy.visitcpdhomepage();
});

Then("the {string} has link to {string}", (id, link) => {
    cy.get('[datatestid="' + id + '"]').should(
        "have.attr",
        "href",
        link
    );
});