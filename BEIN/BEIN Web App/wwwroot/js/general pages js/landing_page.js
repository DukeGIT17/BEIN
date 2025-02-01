const iElements = document.querySelectorAll('.sector-card i');
const linkElement = document.createElement("link");

linkElement.setAttribute("rel", "stylesheet");
linkElement.setAttribute("href", "/css/general pages css/landing_page.css");

document.head.appendChild(linkElement);

document.addEventListener('DOMContentLoaded', function () {
    iElements.forEach(function (i) {
        if (i.className.startsWith('fa')) {
            i.style.margin = '1rem 0';
        }
    });
});

function scrollToSection(sectionId) {
    document.getElementById(sectionId).scrollIntoView({ behavior: 'smooth', block: 'start' });
}