class FeedbackControl {
    #root
    #isUsefulYesRadioButton
    #isUsefulNoRadioButton
    #pageInput
    #feedbackForm
    #cancelLink
    #isUsefulQuestionGroup
    #commentsInput
    #commentsFormGroup

    constructor(root) {
        this.#root = root
    }

    init = () => {
        this.#feedbackForm = this.#root.querySelector("[data-module-id=feedbackForm]")
        this.#cancelLink = this.#root.querySelector("[data-module-id=cancelLink]")
        this.#isUsefulYesRadioButton = this.#root.querySelector("[data-module-id=isUsefulYes]")
        this.#isUsefulNoRadioButton = this.#root.querySelector("[data-module-id=isUsefulNo]")
        this.#isUsefulQuestionGroup = this.#root.querySelector("[data-module-id=isUsefulQuestionGroup]")
        this.#pageInput = this.#root.querySelector("[data-module-id=page]")
        this.#commentsInput = this.#root.querySelector("[data-module-id=comments]")
        this.#commentsFormGroup = this.#root.querySelector("[data-module-id=commentsFormGroup]")

        // Initialise the event handlers
        this.#feedbackForm.addEventListener("submit", this.#handleFormSubmit)
        this.#feedbackForm.addEventListener("reset", this.#handleFormReset)
        this.#cancelLink.addEventListener("click", this.#resetForm)

        this.#show(this.#cancelLink)
    }

    #show = element => element.style.display = "block"
    #hide = element => element.style.display = "none"

    #handleFormSubmit = (event) => {
        event.preventDefault()

        if (this.#validateForm()) {
            const data = {
                Page: this.#pageInput.value,
                IsUseful: this.#isUsefulYesRadioButton.checked,
                Comments: this.#commentsInput.value,
            }
            this.#submitFeedback(data)
            this.#root.querySelector("[data-module-id=submitButton]").disabled = true
            this.#hide(this.#root.querySelector("[data-module-id=controlsContainer]"))
            this.#show(this.#root.querySelector("[data-module-id=thankYouMessage]"))
        }
    }

    #handleFormReset = () => {
        // Close the detail
        this.#root.querySelector("[data-module-id=feedbackDetail]").removeAttribute("open")

        // Hide the error messages
        this.#isUsefulQuestionGroup.classList.remove("govuk-form-group--error")
        this.#commentsFormGroup.classList.remove("govuk-form-group--error")
        this.#hide(this.#root.querySelector("[data-module-id=isUsefulErrorMessage]"))
        this.#hide(this.#root.querySelector("[data-module-id=commentsErrorMessage]"))

        // Executes after the form has been reset - resets the character count component
        setTimeout((() => this.#commentsInput.dispatchEvent(new KeyboardEvent("keyup"))), 1);
    }

    #resetForm = (event) => {
        event.preventDefault()
        this.#feedbackForm.reset()
    }

    #submitFeedback = async (data) => {
        try {
            await fetch("/api/feedback", {
                method: "POST",
                mode: "cors",
                cache: "no-cache",
                credentials: "same-origin",
                headers: {
                    "Content-Type": "application/json",
                    RequestVerificationToken: this.#root.querySelector("[name=__RequestVerificationToken]").value
                },
                redirect: "error",
                referrerPolicy: "same-origin",
                body: JSON.stringify(data),
            });
        } catch (e) {
            console.error(e)
        }
    }

    #validateForm = () => {
        let isValid = true
        if (this.#isUsefulYesRadioButton.checked === false && this.#isUsefulNoRadioButton.checked === false) {
            isValid = false
            this.#isUsefulQuestionGroup.classList.add("govuk-form-group--error")
            this.#show(this.#root.querySelector("[data-module-id=isUsefulErrorMessage]"))
        } else {
            this.#isUsefulQuestionGroup.classList.remove("govuk-form-group--error")
            this.#hide(this.#root.querySelector("[data-module-id=isUsefulErrorMessage]"))
        }

        if (this.#commentsInput.value.length > 400) {
            isValid = false
            this.#commentsFormGroup.classList.add("govuk-form-group--error")
            this.#show(this.#root.querySelector("[data-module-id=commentsErrorMessage]"))
        } else {
            this.#commentsFormGroup.classList.remove("govuk-form-group--error")
            this.#hide(this.#root.querySelector("[data-module-id=commentsErrorMessage]"))
        }

        return isValid
    }
}

document.addEventListener("DOMContentLoaded", () => {
    const controls = document.querySelectorAll("[data-module=feedback-module]")
    controls.forEach(control => {
        new FeedbackControl(control).init()
    })
})