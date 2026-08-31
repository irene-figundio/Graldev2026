// Sticky Header scroll styling & Back to top button visibility toggle
window.addEventListener('scroll', () => {
    const header = document.querySelector('header.site-header');
    if (header) {
        if (window.scrollY > 50) {
            header.classList.add('scrolled');
        } else {
            header.classList.remove('scrolled');
        }
    }

    const backToTopBtn = document.getElementById('backToTop');
    if (backToTopBtn) {
        if (window.scrollY > 300) {
            backToTopBtn.classList.add('visible');
        } else {
            backToTopBtn.classList.remove('visible');
        }
    }
}, { passive: true });

document.addEventListener('DOMContentLoaded', () => {
    // Mobile Hamburger menu toggle
    const hamburger = document.querySelector('.hamburger');
    const mainNav = document.querySelector('.main-nav');

    if (hamburger && mainNav) {
        hamburger.addEventListener('click', () => {
            const expanded = hamburger.getAttribute('aria-expanded') === 'true' || false;
            hamburger.setAttribute('aria-expanded', !expanded);
            mainNav.classList.toggle('active');
        });
    }

    // Dropdown menu responsive toggle for keyboard & touch
    const dropdownTriggers = document.querySelectorAll('.dropdown-trigger');
    dropdownTriggers.forEach(trigger => {
        trigger.addEventListener('click', (e) => {
            if (window.innerWidth <= 992) {
                e.preventDefault();
                const parent = trigger.parentElement;
                if (parent) {
                    parent.classList.toggle('active');
                    const expanded = trigger.getAttribute('aria-expanded') === 'true' || false;
                    trigger.setAttribute('aria-expanded', !expanded);
                }
            }
        });

        trigger.addEventListener('keydown', (e) => {
            if (e.key === 'Escape') {
                const parent = trigger.parentElement;
                if (parent) {
                    parent.classList.remove('active');
                    trigger.setAttribute('aria-expanded', 'false');
                }
            }
        });
    });

    // Theme switch toggle
    const themeBtn = document.getElementById('themeToggle');
    if (themeBtn) {
        themeBtn.addEventListener('click', () => {
            const currentTheme = document.documentElement.getAttribute('data-theme') || 'light';
            const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
            document.documentElement.setAttribute('data-theme', newTheme);
            localStorage.setItem('graldev-theme', newTheme);
        });
    }

    // Back to top button smooth scroll
    const backToTopBtn = document.getElementById('backToTop');
    if (backToTopBtn) {
        backToTopBtn.addEventListener('click', () => {
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        });
    }
});
