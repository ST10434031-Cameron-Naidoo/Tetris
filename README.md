# C# WPF Tetris Game

## Overview
Fully functional Tetris game in C# using WPF without an external game engine. The project implements core game mechanics, rendering, and UI from scratch.

## Key Implementations

### Game Logic
- GameGrid class for managing grid state and row clearing
- Block representation with rotation mechanics for all 7 Tetris pieces
- Position class for tracking block movement
- GameState class managing overall game state and piece spawning

### Game Mechanics
- Block movement and collision detection
- Hard drop feature for instant piece placement
- Hold feature to store and swap the current piece
- Ghost block preview showing where pieces will land
- Next block preview UI
- Progressive difficulty with increasing fall speed

### Rendering & UI
- Custom drawing of game grid and blocks using WPF canvas
- Asset loading and sprite management
- Real-time score tracking
- Game over detection and restart functionality
- Responsive keyboard input handling for game controls

## Technology Stack
- **Language:** C#
- **Framework:** WPF (Windows Presentation Foundation)
- **Game Engine:** None (built from scratch)
