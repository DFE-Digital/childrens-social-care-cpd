class MobileFiltersModal {
    constructor($module) {
        this.$module = $module;
    }

    init() {
        this.$wrapper = document.getElementById("facets-wrapper")
        if (!this.$wrapper) return

        this.$module.addEventListener('click', this.handleClick.bind(this))
    }

    handleClick(event) {
        event.preventDefault()
        this.$wrapper.classList.toggle('facets--visible')
        this.$module.textContent = this.$wrapper.classList.contains("facets--visible") ? "Hide filters" : "Show filters"
    }
}


document.addEventListener("DOMContentLoaded", function () {
    const $mobileFiltersButton = document.getElementById('mobile-filters-button')
    if ($mobileFiltersButton) {
        new MobileFiltersModal($mobileFiltersButton).init()
    }

    const input = document.getElementById('searchTermInput')
    input.addEventListener("keydown", (e) => {
        if (e.key === "Enter") {
            document.getElementById('sortOrder').value = 2
            return false
        }
    })
})