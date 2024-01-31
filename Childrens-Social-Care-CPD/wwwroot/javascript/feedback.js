let submitButton

const show = (element) => element.style.display = "block"
const hide = (element) => element.style.display = "none"

addEventListener("DOMContentLoaded", () => {
    submitButton = document.getElementById("submitButton")
    document.getElementById("isUsefulYes").removeAttribute("required")
    document.getElementById("feedbackForm").addEventListener("submit", handleFormSubmit)
    show(document.getElementById("cancelButton"))
})

function handleFormSubmit(event) {
    const isValid = validateForm()
    try {
        if (isValid) {
            document.getElementById("isUsefulQuestionGroup").classList.remove("govuk-form-group--error")
            hide(document.getElementById("was-useful-error-message"))
            submitFeedback()
        } else {
            document.getElementById("isUsefulQuestionGroup").classList.add("govuk-form-group--error")
            show(document.getElementById("was-useful-error-message"))
        }
    }
    finally {
        event.preventDefault()
    }
}

async function submitFeedback() {
    const data = {
        Page: document.getElementById("page").value,
        IsUseful: document.getElementById("isUsefulYes").checked,
        Comments: document.getElementById("feebackText").value,
    }

    submitButton.disabled = true
    try {
        await fetch("/api/feedback", {
            method: "POST",
            mode: "cors",
            cache: "no-cache",
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
            },
            redirect: "error",
            referrerPolicy: "same-origin",
            body: JSON.stringify(data),
        });
    }
    finally {
        hide(document.getElementById("controlsContainer"))
        show(document.getElementById("thankYouMessage"))
    }
    return false
}

function resetForm() {
    document.getElementById("feedback-control").removeAttribute("open")
    document.getElementById("isUsefulYes").checked = false
    document.getElementById("isUsefulNo").checked = false
    document.getElementById("feebackText").value = ""
    document.getElementById("isUsefulQuestionGroup").classList.remove("govuk-form-group--error")
    hide(document.getElementById("was-useful-error-message"))
    return false
}

function validateForm() {
    return document.getElementById("isUsefulYes").checked === true || document.getElementById("isUsefulNo").checked === true
}