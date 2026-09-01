/**
 * Graldev Tetris - Interactive Easter Egg Minigame
 * Architecture: Clean Vanilla JS Module with Web Audio API, Canvas HTML5, & Touch Controls
 */

(function () {
    'use strict';

    // Grid Dimensions
    const COLS = 10;
    const ROWS = 20;

    // Tetramino Definitions (7 Standard Shapes with Graldev Palette & Styles)
    const SHAPES = {
        I: {
            matrix: [
                [0, 0, 0, 0],
                [1, 1, 1, 1],
                [0, 0, 0, 0],
                [0, 0, 0, 0]
            ],
            color: '#d4af66',      // Champagne Gold
            borderColor: '#f5e4bc',
            codeLabel: 'code'
        },
        J: {
            matrix: [
                [1, 0, 0],
                [1, 1, 1],
                [0, 0, 0]
            ],
            color: '#104928',      // Deep Emerald Green
            borderColor: '#1ed17c',
            codeLabel: 'data'
        },
        L: {
            matrix: [
                [0, 0, 1],
                [1, 1, 1],
                [0, 0, 0]
            ],
            color: '#c5a059',      // Satin Gold
            borderColor: '#ebd8b0',
            codeLabel: 'cloud'
        },
        O: {
            matrix: [
                [1, 1],
                [1, 1]
            ],
            color: '#11ab64',      // Graldev Accent Green
            borderColor: '#24f293',
            codeLabel: 'ai'
        },
        S: {
            matrix: [
                [0, 1, 1],
                [1, 1, 0],
                [0, 0, 0]
            ],
            color: '#008a26',      // Brand Green
            borderColor: '#1ed17c',
            codeLabel: 'sys'
        },
        T: {
            matrix: [
                [0, 1, 0],
                [1, 1, 1],
                [0, 0, 0]
            ],
            color: '#1e3828',      // Dark Greige/Glass Green
            borderColor: '#d4af66',
            codeLabel: 'api'
        },
        Z: {
            matrix: [
                [1, 1, 0],
                [0, 1, 1],
                [0, 0, 0]
            ],
            color: '#e2d2b1',      // Soft Champagne Light
            borderColor: '#ffffff',
            codeLabel: 'web'
        }
    };

    const SHAPE_NAMES = ['I', 'J', 'L', 'O', 'S', 'T', 'Z'];

    // Game Engine State
    let canvas, ctx;
    let nextCanvas, nextCtx;
    let grid = [];
    let currentPiece = null;
    let nextPiece = null;
    let bag = [];

    let score = 0;
    let level = 1;
    let lines = 0;
    let highScore = 0;

    let gameRunning = false;
    let isPaused = false;
    let gameOver = false;

    let dropCounter = 0;
    let lastTime = 0;
    let animationFrameId = null;

    let isMuted = false;
    let audioCtx = null;

    // Web Audio Synthesizer
    function getAudioContext() {
        if (!audioCtx) {
            const AudioContextClass = window.AudioContext || window.webkitAudioContext;
            if (AudioContextClass) {
                audioCtx = new AudioContextClass();
            }
        }
        if (audioCtx && audioCtx.state === 'suspended') {
            audioCtx.resume();
        }
        return audioCtx;
    }

    function playTone(freq, type = 'sine', duration = 0.08, vol = 0.15) {
        if (isMuted) return;
        try {
            const ctxAudio = getAudioContext();
            if (!ctxAudio) return;

            const osc = ctxAudio.createOscillator();
            const gain = ctxAudio.createGain();

            osc.type = type;
            osc.frequency.setValueAtTime(freq, ctxAudio.currentTime);

            gain.gain.setValueAtTime(vol, ctxAudio.currentTime);
            gain.gain.exponentialRampToValueAtTime(0.001, ctxAudio.currentTime + duration);

            osc.connect(gain);
            gain.connect(ctxAudio.destination);

            osc.start();
            osc.stop(ctxAudio.currentTime + duration);
        } catch (e) {
            // Audio context restriction silently ignored
        }
    }

    function playSound(effect) {
        if (isMuted) return;

        switch (effect) {
            case 'move':
                playTone(220, 'sine', 0.04, 0.08);
                break;
            case 'rotate':
                playTone(330, 'triangle', 0.06, 0.12);
                break;
            case 'drop':
                playTone(180, 'square', 0.05, 0.1);
                break;
            case 'hardDrop':
                playTone(120, 'sawtooth', 0.12, 0.2);
                break;
            case 'clear':
                playTone(523.25, 'sine', 0.1, 0.2); // C5
                setTimeout(() => playTone(659.25, 'sine', 0.12, 0.2), 60); // E5
                setTimeout(() => playTone(783.99, 'sine', 0.15, 0.2), 120); // G5
                break;
            case 'levelUp':
                playTone(440, 'triangle', 0.08, 0.2);
                setTimeout(() => playTone(554.37, 'triangle', 0.08, 0.2), 80);
                setTimeout(() => playTone(659.25, 'triangle', 0.08, 0.2), 160);
                setTimeout(() => playTone(880, 'triangle', 0.2, 0.25), 240);
                break;
            case 'gameOver':
                playTone(400, 'sawtooth', 0.15, 0.2);
                setTimeout(() => playTone(300, 'sawtooth', 0.15, 0.2), 150);
                setTimeout(() => playTone(200, 'sawtooth', 0.25, 0.2), 300);
                break;
        }
    }

    // High Score Persistence
    function loadHighScore() {
        try {
            const saved = localStorage.getItem('graldev-tetris-highscore');
            highScore = saved ? parseInt(saved, 10) || 0 : 0;
        } catch (e) {
            highScore = 0;
        }
        updateScoreUI();
    }

    function saveHighScore() {
        if (score > highScore) {
            highScore = score;
            try {
                localStorage.setItem('graldev-tetris-highscore', highScore.toString());
            } catch (e) { }
            updateScoreUI();
            return true;
        }
        return false;
    }

    // Bag Randomizer for Piece Generation
    function getNextFromBag() {
        if (bag.length === 0) {
            bag = [...SHAPE_NAMES];
            // Fisher-Yates shuffle
            for (let i = bag.length - 1; i > 0; i--) {
                const j = Math.floor(Math.random() * (i + 1));
                [bag[i], bag[j]] = [bag[j], bag[i]];
            }
        }
        const name = bag.pop();
        const shapeDef = SHAPES[name];

        return {
            name: name,
            matrix: shapeDef.matrix.map(row => [...row]),
            color: shapeDef.color,
            borderColor: shapeDef.borderColor,
            codeLabel: shapeDef.codeLabel,
            x: Math.floor((COLS - shapeDef.matrix[0].length) / 2),
            y: 0
        };
    }

    // Create Empty Board
    function createGrid() {
        return Array.from({ length: ROWS }, () => Array(COLS).fill(null));
    }

    // Collision Detection
    function collide(gridBoard, piece) {
        const m = piece.matrix;
        for (let r = 0; r < m.length; r++) {
            for (let c = 0; c < m[r].length; c++) {
                if (m[r][c] !== 0) {
                    const newX = piece.x + c;
                    const newY = piece.y + r;

                    if (newX < 0 || newX >= COLS || newY >= ROWS) {
                        return true;
                    }

                    if (newY >= 0 && gridBoard[newY][newX] !== null) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // Lock Piece to Grid
    function mergePiece() {
        const m = currentPiece.matrix;
        for (let r = 0; r < m.length; r++) {
            for (let c = 0; c < m[r].length; c++) {
                if (m[r][c] !== 0) {
                    const boardY = currentPiece.y + r;
                    const boardX = currentPiece.x + c;
                    if (boardY >= 0 && boardY < ROWS && boardX >= 0 && boardX < COLS) {
                        grid[boardY][boardX] = {
                            color: currentPiece.color,
                            borderColor: currentPiece.borderColor
                        };
                    }
                }
            }
        }
    }

    // Rotate Matrix Clockwise
    function rotateMatrix(matrix) {
        const N = matrix.length;
        const result = Array.from({ length: N }, () => Array(N).fill(0));
        for (let r = 0; r < N; r++) {
            for (let c = 0; c < N; c++) {
                result[c][N - 1 - r] = matrix[r][c];
            }
        }
        return result;
    }

    // Player Actions
    function moveLeft() {
        if (!gameRunning || isPaused || gameOver) return;
        currentPiece.x--;
        if (collide(grid, currentPiece)) {
            currentPiece.x++;
        } else {
            playSound('move');
            draw();
        }
    }

    function moveRight() {
        if (!gameRunning || isPaused || gameOver) return;
        currentPiece.x++;
        if (collide(grid, currentPiece)) {
            currentPiece.x--;
        } else {
            playSound('move');
            draw();
        }
    }

    function rotatePiece() {
        if (!gameRunning || isPaused || gameOver) return;
        const originalMatrix = currentPiece.matrix;
        const rotated = rotateMatrix(currentPiece.matrix);
        currentPiece.matrix = rotated;

        // Wall kick attempt
        let offset = 1;
        while (collide(grid, currentPiece)) {
            currentPiece.x += offset;
            offset = -(offset + (offset > 0 ? 1 : -1));
            if (offset > currentPiece.matrix[0].length) {
                currentPiece.matrix = originalMatrix;
                currentPiece.x -= (offset - 1);
                return;
            }
        }

        playSound('rotate');
        draw();
    }

    function softDrop() {
        if (!gameRunning || isPaused || gameOver) return;
        currentPiece.y++;
        if (collide(grid, currentPiece)) {
            currentPiece.y--;
            lockAndSpawn();
        } else {
            score += 1;
            updateScoreUI();
            playSound('drop');
            dropCounter = 0;
            draw();
        }
    }

    function hardDrop() {
        if (!gameRunning || isPaused || gameOver) return;
        let dropDistance = 0;
        while (!collide(grid, currentPiece)) {
            currentPiece.y++;
            dropDistance++;
        }
        currentPiece.y--;
        dropDistance--;

        score += dropDistance * 2;
        updateScoreUI();
        playSound('hardDrop');
        lockAndSpawn();
        draw();
    }

    function lockAndSpawn() {
        mergePiece();
        clearLines();

        currentPiece = nextPiece;
        nextPiece = getNextFromBag();
        drawNextPiece();

        if (collide(grid, currentPiece)) {
            handleGameOver();
        }
    }

    // Line Clearing & Scoring
    function clearLines() {
        let linesClearedCount = 0;

        for (let r = ROWS - 1; r >= 0; r--) {
            let isComplete = true;
            for (let c = 0; c < COLS; c++) {
                if (grid[r][c] === null) {
                    isComplete = false;
                    break;
                }
            }

            if (isComplete) {
                linesClearedCount++;
                grid.splice(r, 1);
                grid.unshift(Array(COLS).fill(null));
                r++; // Re-check row index after shift
            }
        }

        if (linesClearedCount > 0) {
            lines += linesClearedCount;

            // Score Multiplier per lines cleared
            const linePoints = [0, 100, 300, 500, 800];
            score += (linePoints[linesClearedCount] || 800) * level;

            // Level Progression (Every 10 lines)
            const newLevel = Math.floor(lines / 10) + 1;
            if (newLevel > level) {
                level = newLevel;
                showLevelToast(level);
                playSound('levelUp');
            } else {
                playSound('clear');
            }

            updateScoreUI();
        }
    }

    function getFallInterval() {
        // Smooth speed curve down to a minimum cap of 80ms
        return Math.max(80, 800 - (level - 1) * 60);
    }

    function showLevelToast(lvl) {
        const toast = document.getElementById('tetrisLevelToast');
        const lvlNum = document.getElementById('tetrisToastLevelNum');
        if (toast && lvlNum) {
            lvlNum.textContent = lvl < 10 ? '0' + lvl : lvl;
            toast.classList.add('show');
            setTimeout(() => {
                toast.classList.remove('show');
            }, 1200);
        }
    }

    function handleGameOver() {
        gameOver = true;
        gameRunning = false;
        cancelAnimationFrame(animationFrameId);

        playSound('gameOver');
        const isNewHigh = saveHighScore();

        const finalScoreEl = document.getElementById('tetrisFinalScore');
        const finalLevelEl = document.getElementById('tetrisFinalLevel');
        const finalLinesEl = document.getElementById('tetrisFinalLines');
        const newHighBadge = document.getElementById('tetrisNewHighScoreBadge');
        const gameOverOverlay = document.getElementById('tetrisGameOverScreen');

        if (finalScoreEl) finalScoreEl.textContent = score;
        if (finalLevelEl) finalLevelEl.textContent = level;
        if (finalLinesEl) finalLinesEl.textContent = lines;

        if (newHighBadge) {
            newHighBadge.style.display = isNewHigh ? 'block' : 'none';
        }

        if (gameOverOverlay) {
            gameOverOverlay.classList.remove('hidden');
        }
    }

    // Drawing Logic (Canvas HTML5)
    function drawBlock(context, x, y, size, color, borderColor, isGhost = false) {
        const pad = 1.5;
        const radius = 3;

        context.save();

        if (isGhost) {
            context.strokeStyle = color;
            context.lineWidth = 1.5;
            context.setLineDash([3, 3]);
            context.strokeRect(x * size + pad, y * size + pad, size - pad * 2, size - pad * 2);
        } else {
            // Main fill block with subtle bevel
            context.fillStyle = color;
            context.beginPath();
            context.roundRect(x * size + pad, y * size + pad, size - pad * 2, size - pad * 2, radius);
            context.fill();

            // Inner Highlight Border
            context.strokeStyle = borderColor || 'rgba(255,255,255,0.3)';
            context.lineWidth = 1;
            context.stroke();

            // Subtle Graldev Tech Pixel Accents
            context.fillStyle = 'rgba(255, 255, 255, 0.15)';
            context.fillRect(x * size + pad + 2, y * size + pad + 2, (size - pad * 2) * 0.35, 2);
        }

        context.restore();
    }

    function getGhostY() {
        if (!currentPiece) return 0;
        let ghostY = currentPiece.y;
        while (!collide(grid, { ...currentPiece, y: ghostY + 1 })) {
            ghostY++;
        }
        return ghostY;
    }

    function draw() {
        if (!ctx) return;

        const blockSize = canvas.width / COLS;

        // Clear Canvas
        ctx.fillStyle = '#030805';
        ctx.fillRect(0, 0, canvas.width, canvas.height);

        // Draw Subtle Grid Background
        ctx.strokeStyle = 'rgba(17, 171, 100, 0.05)';
        ctx.lineWidth = 1;
        for (let r = 0; r <= ROWS; r++) {
            ctx.beginPath();
            ctx.moveTo(0, r * blockSize);
            ctx.lineTo(canvas.width, r * blockSize);
            ctx.stroke();
        }
        for (let c = 0; c <= COLS; c++) {
            ctx.beginPath();
            ctx.moveTo(c * blockSize, 0);
            ctx.lineTo(c * blockSize, canvas.height);
            ctx.stroke();
        }

        // Draw Grid Filled Blocks
        for (let r = 0; r < ROWS; r++) {
            for (let c = 0; c < COLS; c++) {
                if (grid[r][c] !== null) {
                    drawBlock(ctx, c, r, blockSize, grid[r][c].color, grid[r][c].borderColor);
                }
            }
        }

        // Draw Ghost Piece
        if (currentPiece && gameRunning && !isPaused && !gameOver) {
            const ghostY = getGhostY();
            const m = currentPiece.matrix;
            for (let r = 0; r < m.length; r++) {
                for (let c = 0; c < m[r].length; c++) {
                    if (m[r][c] !== 0) {
                        drawBlock(ctx, currentPiece.x + c, ghostY + r, blockSize, 'rgba(197, 160, 89, 0.4)', null, true);
                    }
                }
            }
        }

        // Draw Active Current Piece
        if (currentPiece && gameRunning && !gameOver) {
            const m = currentPiece.matrix;
            for (let r = 0; r < m.length; r++) {
                for (let c = 0; c < m[r].length; c++) {
                    if (m[r][c] !== 0) {
                        drawBlock(ctx, currentPiece.x + c, currentPiece.y + r, blockSize, currentPiece.color, currentPiece.borderColor);
                    }
                }
            }
        }
    }

    function drawNextPiece() {
        if (!nextCtx || !nextPiece) return;

        nextCtx.fillStyle = '#030805';
        nextCtx.fillRect(0, 0, nextCanvas.width, nextCanvas.height);

        const m = nextPiece.matrix;
        const size = 18;
        const offsetX = (nextCanvas.width - m[0].length * size) / 2;
        const offsetY = (nextCanvas.height - m.length * size) / 2;

        for (let r = 0; r < m.length; r++) {
            for (let c = 0; c < m[r].length; c++) {
                if (m[r][c] !== 0) {
                    const pad = 1;
                    nextCtx.fillStyle = nextPiece.color;
                    nextCtx.beginPath();
                    nextCtx.roundRect(offsetX + c * size + pad, offsetY + r * size + pad, size - pad * 2, size - pad * 2, 2);
                    nextCtx.fill();

                    nextCtx.strokeStyle = nextPiece.borderColor;
                    nextCtx.lineWidth = 1;
                    nextCtx.stroke();
                }
            }
        }
    }

    function updateScoreUI() {
        const scoreEl = document.getElementById('tetrisScore');
        const levelEl = document.getElementById('tetrisLevel');
        const linesEl = document.getElementById('tetrisLines');
        const highEl = document.getElementById('tetrisHighScore');
        const startHighEl = document.getElementById('tetrisStartHighScore');

        if (scoreEl) scoreEl.textContent = score;
        if (levelEl) levelEl.textContent = level;
        if (linesEl) linesEl.textContent = lines;
        if (highEl) highEl.textContent = highScore;
        if (startHighEl) startHighEl.textContent = highScore;
    }

    // Main Game Loop
    function update(time = 0) {
        if (!gameRunning || isPaused || gameOver) return;

        const deltaTime = time - lastTime;
        lastTime = time;

        dropCounter += deltaTime;
        if (dropCounter > getFallInterval()) {
            currentPiece.y++;
            if (collide(grid, currentPiece)) {
                currentPiece.y--;
                lockAndSpawn();
            }
            dropCounter = 0;
        }

        draw();
        animationFrameId = requestAnimationFrame(update);
    }

    // Game Lifecycle Management
    function startGame() {
        grid = createGrid();
        bag = [];
        score = 0;
        level = 1;
        lines = 0;
        gameOver = false;
        isPaused = false;
        gameRunning = true;

        currentPiece = getNextFromBag();
        nextPiece = getNextFromBag();

        updateScoreUI();
        drawNextPiece();

        document.getElementById('tetrisStartScreen')?.classList.add('hidden');
        document.getElementById('tetrisPauseScreen')?.classList.add('hidden');
        document.getElementById('tetrisGameOverScreen')?.classList.add('hidden');

        lastTime = performance.now();
        dropCounter = 0;
        if (animationFrameId) cancelAnimationFrame(animationFrameId);
        animationFrameId = requestAnimationFrame(update);
    }

    function togglePause() {
        if (!gameRunning || gameOver) return;

        isPaused = !isPaused;
        const pauseScreen = document.getElementById('tetrisPauseScreen');
        const pauseBtn = document.getElementById('tetrisPauseBtn');

        if (isPaused) {
            pauseScreen?.classList.remove('hidden');
            if (pauseBtn) pauseBtn.textContent = 'RESUME';
            if (animationFrameId) cancelAnimationFrame(animationFrameId);
        } else {
            pauseScreen?.classList.add('hidden');
            if (pauseBtn) pauseBtn.textContent = 'PAUSE';
            lastTime = performance.now();
            animationFrameId = requestAnimationFrame(update);
        }
    }

    function openModal() {
        const modal = document.getElementById('graldevTetrisModal');
        if (!modal) return;

        modal.classList.add('active');
        document.body.classList.add('tetris-open');

        getAudioContext();
        loadHighScore();

        if (!gameRunning && !gameOver) {
            document.getElementById('tetrisStartScreen')?.classList.remove('hidden');
            draw();
        } else if (isPaused) {
            togglePause();
        }
    }

    function closeModal() {
        const modal = document.getElementById('graldevTetrisModal');
        if (!modal) return;

        modal.classList.remove('active');
        document.body.classList.remove('tetris-open');

        if (gameRunning && !isPaused && !gameOver) {
            togglePause();
        }
    }

    // Event Listeners Initialization
    function setupEventListeners() {
        // Floating Launcher Button
        const launcherBtn = document.getElementById('graldevTetrisBtn');
        if (launcherBtn) {
            launcherBtn.addEventListener('click', openModal);
        }

        // Close & Mute Header Buttons
        document.getElementById('tetrisCloseBtn')?.addEventListener('click', closeModal);

        const muteBtn = document.getElementById('tetrisMuteBtn');
        const muteIcon = document.getElementById('tetrisMuteIcon');
        if (muteBtn) {
            try {
                isMuted = localStorage.getItem('graldev-tetris-muted') === 'true';
                if (muteIcon) muteIcon.textContent = isMuted ? '🔇' : '🔊';
            } catch (e) { }

            muteBtn.addEventListener('click', () => {
                isMuted = !isMuted;
                if (muteIcon) muteIcon.textContent = isMuted ? '🔇' : '🔊';
                try {
                    localStorage.setItem('graldev-tetris-muted', isMuted.toString());
                } catch (e) { }
            });
        }

        // Screen Buttons
        document.getElementById('tetrisStartBtn')?.addEventListener('click', startGame);
        document.getElementById('tetrisResumeBtn')?.addEventListener('click', togglePause);
        document.getElementById('tetrisRestartBtn')?.addEventListener('click', startGame);
        document.getElementById('tetrisPauseBtn')?.addEventListener('click', togglePause);
        document.getElementById('tetrisResetBtn')?.addEventListener('click', startGame);

        // Click outside modal container to close
        const modalOverlay = document.getElementById('graldevTetrisModal');
        if (modalOverlay) {
            modalOverlay.addEventListener('click', (e) => {
                if (e.target === modalOverlay) {
                    closeModal();
                }
            });
        }

        // Desktop Keyboard Navigation
        window.addEventListener('keydown', (e) => {
            const modal = document.getElementById('graldevTetrisModal');
            if (!modal || !modal.classList.contains('active')) return;

            if (e.key === 'Escape') {
                e.preventDefault();
                closeModal();
                return;
            }

            if (!gameRunning || gameOver) return;

            switch (e.key) {
                case 'ArrowLeft':
                case 'a':
                case 'A':
                    e.preventDefault();
                    moveLeft();
                    break;
                case 'ArrowRight':
                case 'd':
                case 'D':
                    e.preventDefault();
                    moveRight();
                    break;
                case 'ArrowUp':
                case 'w':
                case 'W':
                case 'x':
                case 'X':
                    e.preventDefault();
                    rotatePiece();
                    break;
                case 'ArrowDown':
                case 's':
                case 'S':
                    e.preventDefault();
                    softDrop();
                    break;
                case ' ':
                    e.preventDefault();
                    hardDrop();
                    break;
                case 'p':
                case 'P':
                    e.preventDefault();
                    togglePause();
                    break;
            }
        });

        // Mobile Touch Control Handlers
        const attachTouch = (id, action) => {
            const btn = document.getElementById(id);
            if (btn) {
                btn.addEventListener('touchstart', (e) => {
                    e.preventDefault();
                    action();
                }, { passive: false });
                btn.addEventListener('click', (e) => {
                    e.preventDefault();
                    action();
                });
            }
        };

        attachTouch('touchLeft', moveLeft);
        attachTouch('touchRight', moveRight);
        attachTouch('touchRotate', rotatePiece);
        attachTouch('touchSoftDrop', softDrop);
        attachTouch('touchHardDrop', hardDrop);

        // Auto-Pause when losing focus or changing tabs
        window.addEventListener('blur', () => {
            if (gameRunning && !isPaused && !gameOver) {
                togglePause();
            }
        });

        document.addEventListener('visibilitychange', () => {
            if (document.hidden && gameRunning && !isPaused && !gameOver) {
                togglePause();
            }
        });
    }

    // DOM Ready Initialization
    document.addEventListener('DOMContentLoaded', () => {
        canvas = document.getElementById('tetrisCanvas');
        if (canvas) ctx = canvas.getContext('2d');

        nextCanvas = document.getElementById('tetrisNextCanvas');
        if (nextCanvas) nextCtx = nextCanvas.getContext('2d');

        setupEventListeners();
        loadHighScore();
    });
})();
