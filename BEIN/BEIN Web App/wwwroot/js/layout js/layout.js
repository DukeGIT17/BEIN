const username = document.getElementById('username');
const searchField = document.getElementById('search-field');
const searchDisplay = document.getElementById('search-display-div');
const nav = document.querySelectorAll('nav')[0];
const header = document.querySelectorAll('header')[0];
const toggleSBBtn = document.getElementById('toggle-sidebar');
const toggleSBIcon = document.getElementById('toggle-btn-icon');

searchField.addEventListener('focus', async () => {
    await getSearchData();
    searchDisplay.style.display = 'flex'
});

username.addEventListener('mouseover', function () {
    document.getElementById('user-menu').style.display = 'flex';
});

document.addEventListener('click', function (event) {
    const userMenu = document.getElementById('user-menu');
    const searchDiv = document.getElementById('search');

    if (!username.contains(event.target) && !userMenu.contains(event.target)) {
        userMenu.style.display = 'none';
    }

    if (!searchDisplay.contains(event.target) && !searchDiv.contains(event.target)) {
        searchDisplay.style.display = 'none';
    }
});

async function getSearchData() {
    const sectorArea = document.getElementById('sectors-area');
    const softwareArea = document.getElementById('software-area');

    if (sectorArea.childElementCount < 2 && softwareArea.childElementCount < 2) {
        let searchDataCollection;
        const response = await fetch('https://localhost:7012/api.bein.com/Public/SearchData');

        if (response.ok) {



            searchDataCollection = await response.json();
            searchDataCollection.forEach(element => {
                const anchor = document.createElement('a');
                anchor.classList.add('search-item');
                anchor.innerText = element.title;
                if (element.type === 'Sector') {
                    sectorArea.appendChild(anchor);
                } else if (element.type === 'Software') {
                    softwareArea.appendChild(anchor);
                } else {
                    anchor.remove();
                    console.error('Provide data type is neither sector nor software.');
                }
            });
        } else {
            console.error(`Error: '${response.statusText}'! ${await response.text()}`);
        }
    }
}

toggleSBBtn.addEventListener('click', () => {
    if (window.innerWidth > 670) {
        toggleSidebar()
    }
});



function toggleSidebar() {
    if (toggleSBIcon && toggleSBIcon.classList.contains('fa-arrow-left')) {
        if (nav && header) {
            header.style.width = '300px';
            nav.style.display = 'flex';
            toggleSBIcon.classList.replace('fa-arrow-left', 'fa-arrow-right');
        } else if (!nav) {
            alert('nav is null.');
        } else {
            alert('header is null');
        }
    } else if (toggleSBIcon && toggleSBIcon.classList.contains('fa-arrow-right')) {
        if (nav && header) {
            header.style.width = '0';
            nav.style.display = 'none';
            toggleSBIcon.classList.replace('fa-arrow-right', 'fa-arrow-left');
        } else if (!nav) {
            alert('nav is null.');
        } else {
            alert('header is null');
        }
    } else {
        alert('sidebar toggle icon is null');
    }
}

toggleSBBtn.addEventListener('click', () => {
    const hStyle = header.style;
    if (window.innerWidth <= 670 && toggleSBIcon && toggleSBBtn && (hStyle.width === '0' || hStyle.width === '0px' || hStyle.width === '')) {
        hStyle.width = '100%';
        nav.style.display = 'flex';
        toggleSBBtn.classList.add('tsb_670');
        toggleSBBtn.style.left = '0';
        toggleSBIcon.classList.remove('fa-arrow-left');
        toggleSBIcon.classList.add('fa-arrow-right');
    } else if (window.innerWidth <= 670 && toggleSBIcon && toggleSBBtn && hStyle.width > '0') {
        hStyle.width = '0';
        nav.style.display = 'none';
        toggleSBBtn.classList.remove('tsb_670');
        toggleSBBtn.style.left = '-50px';
        toggleSBIcon.classList.remove('fa-arrow-right');
        toggleSBIcon.classList.add('fa-arrow-left');
    }
});

window.addEventListener('resize', () => {
    if (window.innerWidth >= 950) {
        header.style.width = '100%';
        nav.style.display = 'flex';
    } else {
        header.style.width = '0';
        nav.style.display = 'none';
        toggleSBBtn.classList.toggle('tsb_670', false);
        toggleSBBtn.style.left = '-50px';
        toggleSBIcon.classList.remove('fa-arrow-right');
        toggleSBIcon.classList.add('fa-arrow-left');
    }
});