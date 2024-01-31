let submitButton
let controlsContainer
let thankYouMessage
let feedbackForm

const show = (element) => element.style.display = "block"
const hide = (element) => element.style.display = "none"

addEventListener("DOMContentLoaded", () => {
    submitButton = document.getElementById("submitButton")
    controlsContainer = document.getElementById("controlsContainer")
    thankYouMessage = document.getElementById("thankYouMessage")
    feedbackForm = document.getElementById("feedbackForm")

    feedbackForm.addEventListener("submit", handleFormSubmit)

    show(document.getElementById("cancelButton"))
})

function handleFormSubmit(event) {
    const isValid = feedbackForm.reportValidity()
    try {
        if (isValid) {
            submitFeedback()
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
        hide(controlsContainer)
        show(thankYouMessage)
    }
    return false
}

function resetForm() {
    document.getElementById("feedback-control").removeAttribute("open")
    document.getElementById("isUsefulYes").checked = false
    document.getElementById("isUsefulNo").checked = false
    document.getElementById("feebackText").value = ""
    return false
}