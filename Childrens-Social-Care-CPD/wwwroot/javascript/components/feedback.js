class FeedbackControl {
    #root
    #isUsefulYesRadioButton
    #isUsefulNoRadioButton
    #pageInput
    #feedbackForm
    #cancelLink
    #isUsefulQuestionGroup

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
                IsUseful: this.#isUsefulYesRadioButton.checked
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
        this.#hide(this.#root.querySelector("[data-module-id=isUsefulErrorMessage]"))
    }

    #resetForm = (event) => {
        event.preventDefault()
        this.#feedbackForm.reset()
    }

    #submitFeedback = async (data) => {
        try {
            console.log('submitting feedback');
            const pageURL = document.location.pathname + document.location.search;
            gtag('event', 'page_rating', {
                'feedback_page_name': pageURL,
                'feedback_score': data.IsUseful ? '1' : '0'
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
        return isValid
    }
}

document.addEventListener("DOMContentLoaded", () => {
    const controls = document.querySelectorAll("[data-module=feedback-module]")
    controls.forEach(control => {
        new FeedbackControl(control).init()
    })
})