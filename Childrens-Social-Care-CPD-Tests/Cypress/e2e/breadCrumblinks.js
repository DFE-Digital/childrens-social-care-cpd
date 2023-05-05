import { Given, Then, When } from "@badeball/cypress-cucumber-preprocessor";


Given("a user has clicked on {string}", (id) => {
    cy.visitcpdhomepage();
    cy.get('[datatestid="' + id + '"]').click();
});


Then("the user clicks on {string} link should redirect to {string}", (id, link) => {
    cy.get('[datatestid="' + id + '"]').should('exist');
    cy.get('[datatestid="' + id + '"]').click();
    cy.url().should('include', link)
});

When("user clicks on {string}", (id) => {
    cy.get('[datatestid="' + id + '"]').click();
});

When("user clicks on sub heading", () => {
    cy.get("h3").click();
});