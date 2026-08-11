/* Warm Minimalist / Soft Tech - Design Preview Interactivity & Motion module */
document.addEventListener('DOMContentLoaded', () => {
    // 1. Mobile Menu Toggle
    const hamburger = document.querySelector('.dp-hamburger');
    const nav = document.querySelector('.dp-nav');

    if (hamburger && nav) {
        hamburger.addEventListener('click', () => {
            nav.classList.toggle('active');
            hamburger.classList.toggle('active');

            // Toggle hamburger bars
            const spans = hamburger.querySelectorAll('span');
            if (hamburger.classList.contains('active')) {
                spans[0].style.transform = 'rotate(45deg) translate(6px, 6px)';
                spans[1].style.opacity = '0';
                spans[2].style.transform = 'rotate(-45deg) translate(5px, -5px)';
            } else {
                spans[0].style.transform = 'none';
                spans[1].style.opacity = '1';
                spans[2].style.transform = 'none';
            }
        });
    }

    // 2. Interactive Glass Layer floating parallax
    const scene = document.querySelector('.dp-visual-scene');
    const layers = document.querySelectorAll('.dp-glass-plate');

    if (scene && layers.length > 0) {
        scene.addEventListener('mousemove', (e) => {
            const rect = scene.getBoundingClientRect();
            const x = e.clientX - rect.left - (rect.width / 2);
            const y = e.clientY - rect.top - (rect.height / 2);

            // Apply different parallax speeds for deep tridimensional feel
            layers.forEach((layer, index) => {
                const depth = (index + 1) * 0.08;
                const rotateY = -32 + (x * depth * 0.1);
                const rotateX = 15 - (y * depth * 0.1);
                const translateZ = (index === 0) ? 100 : (index === 1) ? 20 : -80;
                const translateX = (index === 0) ? -110 : (index === 1) ? 0 : 110;

                layer.style.transform = `rotateY(${rotateY}deg) rotateX(${rotateX}deg) translateZ(${translateZ}px) translateX(${translateX}px)`;
            });
        });

        scene.addEventListener('mouseleave', () => {
            // Restore default values smoothly
            layers.forEach((layer, index) => {
                const translateZ = (index === 0) ? 100 : (index === 1) ? 20 : -80;
                const translateX = (index === 0) ? -110 : (index === 1) ? 0 : 110;
                layer.style.transform = `rotateY(-32deg) rotateX(15deg) translateZ(${translateZ}px) translateX(${translateX}px)`;
            });
        });
    }

    // 3. Simple elegant reveal on scroll
    const sections = document.querySelectorAll('.dp-section');
    const observerOptions = {
        root: null,
        threshold: 0.1,
        rootMargin: '0px'
    };

    const sectionObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
                sectionObserver.unobserve(entry.target);
            }
        });
    }, observerOptions);

    sections.forEach(section => {
        section.style.opacity = '0';
        section.style.transform = 'translateY(30px)';
        section.style.transition = 'opacity 1s cubic-bezier(0.16, 1, 0.3, 1), transform 1s cubic-bezier(0.16, 1, 0.3, 1)';
        sectionObserver.observe(section);
    });
});
