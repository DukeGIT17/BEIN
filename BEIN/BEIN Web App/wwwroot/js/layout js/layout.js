// Main JavaScript for handling interactions
document.addEventListener('DOMContentLoaded', () => {
    // Navigation toggle
    const navToggle = document.getElementById('nav-toggle');
    const navMenu = document.getElementById('nav-menu');

    if (navToggle && navMenu) {
        navToggle.addEventListener('click', () => {
            navMenu.classList.toggle('show-menu');
            navToggle.querySelector('i').classList.toggle('ri-menu-line');
            navToggle.querySelector('i').classList.toggle('ri-close-line');
        });
    }

    // Close menu when clicking outside
    document.addEventListener('click', (e) => {
        if (!e.target.closest('.nav__menu') && !e.target.closest('.nav__toggle')) {
            if (navMenu && navMenu.classList.contains('show-menu')) {
                navMenu.classList.remove('show-menu');
                navToggle.querySelector('i').classList.add('ri-menu-line');
                navToggle.querySelector('i').classList.remove('ri-close-line');
            }
        }
    });

    // Smooth scroll for navigation links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
                // Close mobile menu if open
                if (navMenu && navMenu.classList.contains('show-menu')) {
                    navMenu.classList.remove('show-menu');
                }
            }
        });
    });

    // Header scroll behavior
    const header = document.querySelector('.header');
    let lastScroll = 0;

    window.addEventListener('scroll', () => {
        const currentScroll = window.pageYOffset;

        if (currentScroll <= 0) {
            header.classList.remove('scroll-up');
            return;
        }

        if (currentScroll > lastScroll && !header.classList.contains('scroll-down')) {
            // Scrolling down
            header.classList.remove('scroll-up');
            header.classList.add('scroll-down');
        } else if (currentScroll < lastScroll && header.classList.contains('scroll-down')) {
            // Scrolling up
            header.classList.remove('scroll-down');
            header.classList.add('scroll-up');
        }
        lastScroll = currentScroll;
    });

    // Search functionality
    const searchInput = document.querySelector('.search__input');
    const searchButton = document.querySelector('.search__button');

    if (searchButton && searchInput) {
        searchButton.addEventListener('click', () => {
            const searchTerm = searchInput.value.trim();
            if (searchTerm) {
                // Implement search functionality here
                console.log('Searching for:', searchTerm);
            }
        });

        searchInput.addEventListener('keypress', (e) => {
            if (e.key === 'Enter') {
                const searchTerm = searchInput.value.trim();
                if (searchTerm) {
                    // Implement search functionality here
                    console.log('Searching for:', searchTerm);
                }
            }
        });
    }

    // Intersection Observer for animations
    //const animateOnScroll = new IntersectionObserver((entries) => {
    //    entries.forEach(entry => {
    //        if (entry.isIntersecting) {
    //            entry.target.classList.add('animate');
    //        }
    //    });
    //}, {
    //    threshold: 0.1
    //});

    //// Animate features on scroll
    //document.querySelectorAll('.feature__card').forEach(card => {
    //    animateOnScroll.observe(card);
    //});
});