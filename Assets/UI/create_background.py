#!/usr/bin/env python3
"""
Скрипт для создания фонового изображения для меню игры Bomb It!
Требует установки Pillow: pip install Pillow
"""

try:
    from PIL import Image, ImageDraw
    import os
    
    # Создаем изображение 1920x1080
    width, height = 1920, 1080
    img = Image.new('RGB', (width, height), color='#1a1a2e')
    
    draw = ImageDraw.Draw(img)
    
    # Рисуем градиентный фон (темно-синий к черному)
    for y in range(height):
        r = int(26 + (y / height) * 10)
        g = int(26 + (y / height) * 10)
        b = int(46 + (y / height) * 20)
        draw.line([(0, y), (width, y)], fill=(r, g, b))
    
    # Добавляем эффект взрыва (яркие круги)
    for i in range(15):
        x = (i * 150) % width
        y = (i * 100) % height
        size = 200 + (i % 3) * 50
        # Внешний круг (яркий оранжевый)
        draw.ellipse([x-size, y-size, x+size, y+size], fill=(255, 100, 0, 30))
        # Внутренний круг (очень яркий желтый)
        draw.ellipse([x-size//2, y-size//2, x+size//2, y+size//2], fill=(255, 200, 0, 50))
    
    # Добавляем пиксельные эффекты (искры)
    for i in range(100):
        x = (i * 37) % width
        y = (i * 73) % height
        draw.ellipse([x-3, y-3, x+3, y+3], fill=(255, 255, 100))
    
    # Сохраняем изображение
    output_path = os.path.join(os.path.dirname(__file__), 'menu_background.png')
    img.save(output_path)
    print(f'✓ Фоновое изображение создано: {output_path}')
    print(f'  Размер: {width}x{height} пикселей')
    
except ImportError:
    print("Ошибка: Не установлена библиотека Pillow")
    print("Установите её командой: pip install Pillow")
except Exception as e:
    print(f"Ошибка при создании изображения: {e}")

