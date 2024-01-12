document.addEventListener("DOMContentLoaded", function () {
    const input = document.getElementById('searchTermInput')
    input.addEventListener("keydown", (e) => {
        if (e.key === "Enter") {
            document.getElementById('sortOrder').value = 2
            return false
        }
    })
})