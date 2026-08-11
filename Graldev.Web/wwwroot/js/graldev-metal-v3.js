(() => {
    const sections = Array.from(document.querySelectorAll('.metal-v3-section'));

    if (!sections.length) {
        return;
    }

    const reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)');
    const finePointer = window.matchMedia('(pointer: fine)');
    const controllers = [];

    function createController(section) {
        const svg = section.querySelector('.hero-metal-pattern-svg');
        const shineCircle = section.querySelector('.hero-metal-shine-circle');

        if (!svg || !shineCircle) {
            return null;
        }

        let targetX = 800;
        let targetY = 350;
        let currentX = 800;
        let currentY = 350;
        let animationFrameId = 0;
        let lastFrame = 0;
        let pointerQueued = false;
        let pendingClientX = 0;
        let pendingClientY = 0;

        function clientToSvg(clientX, clientY) {
            const point = svg.createSVGPoint();
            point.x = clientX;
            point.y = clientY;

            const matrix = svg.getScreenCTM();

            if (!matrix) {
                return { x: 800, y: 350 };
            }

            const transformed = point.matrixTransform(matrix.inverse());

            return {
                x: Math.max(0, Math.min(1600, transformed.x)),
                y: Math.max(0, Math.min(1000, transformed.y))
            };
        }

        function setCirclePosition(x, y) {
            shineCircle.setAttribute('cx', x.toFixed(2));
            shineCircle.setAttribute('cy', y.toFixed(2));
        }

        function wakeAnimation() {
            if (animationFrameId !== 0) {
                return;
            }

            lastFrame = performance.now();
            animationFrameId = requestAnimationFrame(animate);
        }

        function animate(now) {
            const deltaTime = Math.min(32, now - lastFrame);
            lastFrame = now;

            const ease = 1 - Math.exp(-0.014 * deltaTime);

            currentX += (targetX - currentX) * ease;
            currentY += (targetY - currentY) * ease;

            setCirclePosition(currentX, currentY);

            const deltaX = targetX - currentX;
            const deltaY = targetY - currentY;

            if (Math.abs(deltaX) > 0.25 || Math.abs(deltaY) > 0.25) {
                animationFrameId = requestAnimationFrame(animate);
                return;
            }

            currentX = targetX;
            currentY = targetY;
            setCirclePosition(currentX, currentY);
            animationFrameId = 0;
        }

        function move() {
            if (reducedMotion.matches) {
                currentX = targetX;
                currentY = targetY;
                setCirclePosition(currentX, currentY);
                return;
            }

            wakeAnimation();
        }

        section.addEventListener('pointermove', event => {
            if (!finePointer.matches) {
                return;
            }

            pendingClientX = event.clientX;
            pendingClientY = event.clientY;

            if (pointerQueued) {
                return;
            }

            pointerQueued = true;

            requestAnimationFrame(() => {
                pointerQueued = false;

                const point = clientToSvg(pendingClientX, pendingClientY);
                targetX = point.x;
                targetY = point.y;

                const rect = section.getBoundingClientRect();

                section.style.setProperty(
                    '--hero-metal-mx',
                    `${((pendingClientX - rect.left) / rect.width) * 100}%`
                );

                section.style.setProperty(
                    '--hero-metal-my',
                    `${((pendingClientY - rect.top) / rect.height) * 100}%`
                );

                move();
            });
        }, { passive: true });

        section.addEventListener('pointerleave', () => {
            if (!finePointer.matches) {
                return;
            }

            targetX = 800;
            targetY = 350;
            move();
        });

        function updateTouchScroll(viewportHeight) {
            if (finePointer.matches) {
                return;
            }

            const rect = section.getBoundingClientRect();

            if (rect.bottom < -80 || rect.top > viewportHeight + 80) {
                return;
            }

            const progress = Math.max(
                0,
                Math.min(
                    1,
                    (viewportHeight - rect.top) / (viewportHeight + rect.height)
                )
            );

            targetX = 170 + progress * 1260;
            targetY = 760 - progress * 520;

            section.style.setProperty(
                '--hero-metal-mx',
                `${15 + progress * 70}%`
            );

            section.style.setProperty(
                '--hero-metal-my',
                `${70 - progress * 40}%`
            );

            move();
        }

        setCirclePosition(currentX, currentY);

        return { updateTouchScroll };
    }

    sections.forEach(section => {
        const controller = createController(section);

        if (controller) {
            controllers.push(controller);
        }
    });

    let scrollFrameQueued = false;

    function updateAllTouchSections() {
        if (finePointer.matches || scrollFrameQueued) {
            return;
        }

        scrollFrameQueued = true;

        requestAnimationFrame(() => {
            scrollFrameQueued = false;
            const viewportHeight = window.innerHeight;

            controllers.forEach(controller => {
                controller.updateTouchScroll(viewportHeight);
            });
        });
    }

    window.addEventListener('scroll', updateAllTouchSections, { passive: true });
    window.addEventListener('resize', updateAllTouchSections, { passive: true });

    updateAllTouchSections();
})();