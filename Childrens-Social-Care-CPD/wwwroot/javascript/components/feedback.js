let submitButton

class FeedbackControl {
    constructor($root) {
        this.$root = $root
    }

    init() {
        this.$submitButton = document.getElementById("submitButton")
        document.getElementById("isUsefulYes").removeAttribute("required")
        document.getElementById("feedbackForm").addEventListener("submit", handleFormSubmit)
        show("cancelButton")
    }
}

document.addEventListener("DOMContentLoaded", () => {
    const controls = document.querySelectorAll("[data-module='feedback-module']")
    controls.forEach(control => {
        const feedbackControl = new FeedbackControl(control)
        feedbackControl.init()
    })
})

const show = (id) => document.getElementById(id).style.display = "block"
const hide = (id) => document.getElementById(id).style.display = "none"

addEventListener("DOMContentLoaded", () => {
    submitButton = document.getElementById("submitButton")
    document.getElementById("isUsefulYes").removeAttribute("required")
    document.getElementById("feedbackForm").addEventListener("submit", handleFormSubmit)
    show("cancelButton")
})

function handleFormSubmit(event) {
    const isValid = validateForm()
    try {
        if (isValid) {
            const data = {
                Page: document.getElementById("page").value,
                IsUseful: document.getElementById("isUsefulYes").checked,
                Comments: document.getElementById("comments").value,
            }
            submitFeedback(data)
            submitButton.disabled = true
            hide("controlsContainer")
            show("thankYouMessage")
        }
    }
    finally {
        event.preventDefault()
    }
}

async function submitFeedback(data) {
    try {
        await fetch("/api/feedback", {
            method: "POST",
            mode: "cors",
            cache: "no-cache",
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
                RequestVerificationToken: document.getElementsByName("__RequestVerificationToken")[0].value
            },
            redirect: "error",
            referrerPolicy: "same-origin",
            body: JSON.stringify(data),
        });
    } catch { }

    return false
}

function resetForm() {
    document.getElementById("feedback-control").removeAttribute("open")
    document.getElementById("isUsefulYes").checked = false
    document.getElementById("isUsefulNo").checked = false
    document.getElementById("comments").value = ""
    document.getElementById("isUsefulQuestionGroup").classList.remove("govuk-form-group--error")
    hide("was-useful-error-message")
    return false
}

function validateForm() {

    let isValid = true

    if (document.getElementById("isUsefulYes").checked === false && document.getElementById("isUsefulNo").checked === false) {
        isValid = false
        document.getElementById("isUsefulQuestionGroup").classList.add("govuk-form-group--error")
        show("isUsefulErrorMessage")
    } else {
        document.getElementById("isUsefulQuestionGroup").classList.remove("govuk-form-group--error")
        hide("isUsefulErrorMessage")
    }

    if (document.getElementById("comments").value.length > 400) {
        isValid = false
        document.getElementById("commentsFormGroup").classList.add("govuk-form-group--error")
        show("commentsErrorMessage")
    } else {
        document.getElementById("commentsFormGroup").classList.remove("govuk-form-group--error")
        hide("commentsErrorMessage")
    }

    return isValid
}