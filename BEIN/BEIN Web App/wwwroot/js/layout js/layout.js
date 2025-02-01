const navToggle = document.getElementById('nav_toggle');
const sideBar = document.getElementById('side-bar');
const username = document.getElementById('username');
const usernameSb = document.getElementById('username-sb');

navToggle.addEventListener('click', function () {
    if (sideBar.style.display === 'none' || sideBar.style.display === '') {
        sideBar.style.display = 'flex';
    } else {
        sideBar.style.display = 'none';
    }
});

window.addEventListener('resize', function () {
    if (this.innerWidth > 950) {
        sideBar.style.display = 'none';
    }
});

if (username) {
    username.addEventListener('mouseover', function () {
        document.getElementById('user-menu').style.display = 'flex';
    });
}

if (usernameSb) {
    usernameSb.addEventListener('mouseover', function () {
        document.getElementById('user-menu-sb').style.display = 'flex';
    });
}

document.addEventListener('click', function (event) {
    const userMenu = document.getElementById('user-menu');
    const userMenuSb = document.getElementById('user-menu-sb');

    if (userMenu) {
        if (!username.contains(event.target) && !userMenu.contains(event.target)) {
            userMenu.style.display = 'none';
        }
    }

    if (userMenuSb) {
        if (!usernameSb.contains(event.target) && !userMenu.contains(event.target)) {
            userMenuSb.style.display = 'none';
        }
    }
});