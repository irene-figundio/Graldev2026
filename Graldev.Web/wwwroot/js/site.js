// Sticky Header scroll styling toggle
window.addEventListener('scroll', () => {
    const header = document.querySelector('header.site-header');
    if (header) {
        if (window.scrollY > 50) {
            header.classList.add('scrolled');
        } else {
            header.classList.remove('scrolled');
        }
    }
}, { passive: true });

// Mobile Hamburger menu toggle
document.addEventListener('DOMContentLoaded', () => {
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
});
