let resetButton;
let submitButton;
let introText;
let questionContainer;
let feedbackContainer;
let thankYouMessage;

addEventListener("DOMContentLoaded", () => {
    submitButton = document.getElementById("submitButton")
    resetButton = document.getElementById("resetButton")
    introText = document.getElementById('introText')
    questionContainer = document.getElementById('questionContainer')
    feedbackContainer = document.getElementById('feedbackContainer')
    thankYouMessage = document.getElementById('thankYouMessage')

    show(document.getElementById("cancelButton"))
    submitButton.removeAttribute("type")

    feedbackStep1()
})

function show(element) {
    element.style.display = "block"
}

function hide(element) {
    element.style.display = "none"
}

function feedbackStep1() {
    show(introText)
    show(questionContainer)
    hide(feedbackContainer)
}

function feedbackStep2() {
    hide(introText)
    hide(questionContainer)
    show(feedbackContainer)
}

async function submitFeedback(event) {
    event.preventDefault()

    const data = {
        Page: document.getElementById("page").value,
        IsUseful: document.getElementById("isUsefulYes").checked,
        Comments: document.getElementById("feebackText").value,
        AdditionalComments: document.getElementById("otherFeedbackText").value,
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
            referrerPolicy: "same-origin", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
            body: JSON.stringify(data), // body data type must match "Content-Type" header
        });
    } finally {
        hide(feedbackContainer)
        show(thankYouMessage)
        return false
    }
}

function resetForm() {
    document.getElementById("isUsefulYes").checked = false
    document.getElementById("isUsefulNo").checked = false

    document.getElementById("feebackText").value = ""
    document.getElementById("otherFeedbackText").value = ""

    feedbackStep1()
    return false
}