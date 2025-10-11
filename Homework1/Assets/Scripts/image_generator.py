from PIL import Image, ImageDraw
import random
import os

width = 1280
height = 960
columns = 20
rows = 15

try:
    script_dir = os.path.dirname(os.path.abspath(__file__))
except NameError:
    script_dir = os.getcwd()

output_dir = os.path.join(script_dir, "generated_patterns")
os.makedirs(output_dir, exist_ok=True)

palettes = [
    [(80,120,20), (100,140,30), (60,100,15), (120,160,40), (90,130,25)],
    [(150,100,20), (180,120,30), (120,80,15), (200,140,40), (160,110,25)],
    [(20,60,100), (40,80,140), (30,77,108), (43,57,75), (60,100,160)],
    [(180,40,40), (200,80,20), (160,60,30), (200,50,50), (220,100,30)],
    [(120,40,180), (100,20,140), (180,40,120), (140,60,160), (160,80,200)],
    [(0,120,120), (20,180,180), (50,150,150), (40,140,160), (60,140,140)],
    [(100,60,20), (120,80,40), (80,50,25), (140,100,60), (110,70,30)],
    [(180,0,180), (0,160,160), (200,200,0), (200,0,100), (100,180,0)],
]


def clamp(v):
    return max(0, min(255, int(v)))

def tint_color(color, factor):
    return tuple(clamp(c + (255 - c) * factor) for c in color)

def shade_color(color, factor):
    return tuple(clamp(c * (1 - factor)) for c in color)

def jitter_color(color, amount=18):
    return tuple(clamp(c + random.randint(-amount, amount)) for c in color)


def expand_palette(palette, target_len=10):
    colors = list(palette)
    i = 0
    while len(colors) < target_len:
        base = palette[i % len(palette)]
        choice = random.choice(['tint', 'shade', 'jitter'])
        if choice == 'tint':
            colors.append(tint_color(base, random.uniform(0.12, 0.45)))
        elif choice == 'shade':
            colors.append(shade_color(base, random.uniform(0.12, 0.45)))
        else:
            colors.append(jitter_color(base, amount=18))
        i += 1
    
    seen = []
    for c in colors:
        if c not in seen:
            seen.append(c)
    return seen


accents = [
    (255, 255, 255),
    (255, 255, 0),
    (0, 255, 255),
    (255, 0, 255),
    (255, 128, 0),
    (128, 255, 0),
]

color_palettes = [expand_palette(p, target_len=10) for p in palettes]

def draw_pattern(pattern_id, color_palette, seed=None):
    if seed is not None:
        random.seed(seed)
    
    img = Image.new("RGB", (width, height), (255,255,255))
    draw = ImageDraw.Draw(img)
    x_incr = width / columns
    y_incr = height / rows
    
    color_index = [0]
    last_colors = []
    
    def get_color():
        if color_index[0] >= len(color_palette) * 3:
            color_index[0] = 0
            random.shuffle(color_palette)
        if random.random() < 0.15:
            accent = random.choice(accents)
            if accent in last_colors[-2:]:
                for _ in range(3):
                    accent = random.choice(accents)
                    if accent not in last_colors[-2:]:
                        break
            last_colors.append(accent)
            if len(last_colors) > 3:
                last_colors.pop(0)
            color_index[0] += 1
            return accent

        available = [c for c in color_palette if c not in last_colors[-2:]]
        if not available:
            available = color_palette

        color = random.choice(available)
        last_colors.append(color)
        if len(last_colors) > 3:
            last_colors.pop(0)

        color_index[0] += 1
        return color
    
    base_type = pattern_id % 12
    
    if base_type == 0:
        for _ in range(int(columns * rows * 1.5)):
            x1, y1 = random.randint(0, width), random.randint(0, height)
            x2, y2 = x1 + random.randint(-100, 100), y1 + random.randint(-100, 100)
            x3, y3 = x1 + random.randint(-100, 100), y1 + random.randint(-100, 100)
            draw.polygon([(x1,y1), (x2,y2), (x3,y3)], fill=get_color())
    
    elif base_type == 1:
        stripe_width = random.randint(30, 80)
        for i in range(0, width + height, stripe_width):
            points = []
            for j in range(0, width, int(x_incr)):
                offset = random.randint(-30, 30)
                points.append((j, i - j + offset))
                points.append((j, i - j + stripe_width + offset))
            if len(points) > 2:
                draw.polygon(points[:len(points)//2*2], fill=get_color())
    
    elif base_type == 2:
        for _ in range(int(columns * rows * 0.8)):
            x, y = random.randint(0, width-100), random.randint(0, height-100)
            w, h = random.randint(40, 150), random.randint(40, 150)
            skew = random.randint(-30, 30)
            draw.polygon([(x, y), (x+w+skew, y), (x+w, y+h), (x-skew, y+h)], fill=get_color())
    
    elif base_type == 3:
        for x in range(0, width, int(x_incr*2)):
            for y in range(0, height, int(y_incr*2)):
                direction = random.choice(['up', 'down', 'left', 'right'])
                size = random.uniform(x_incr, x_incr*2)
                if direction == 'up':
                    draw.polygon([(x+size/2,y), (x+size,y+size), (x,y+size)], fill=get_color())
                elif direction == 'down':
                    draw.polygon([(x,y), (x+size,y), (x+size/2,y+size)], fill=get_color())
                elif direction == 'left':
                    draw.polygon([(x+size,y), (x+size,y+size), (x,y+size/2)], fill=get_color())
                else:
                    draw.polygon([(x,y), (x,y+size), (x+size,y+size/2)], fill=get_color())
    
    elif base_type == 4:
        import math
        for _ in range(int(columns * rows * 0.6)):
            cx, cy = random.randint(0, width), random.randint(0, height)
            radius = random.randint(30, 80)
            rotation = random.uniform(0, 2*math.pi)
            points = []
            for i in range(5):
                angle = rotation + 2 * math.pi * i / 5
                x = cx + radius * math.cos(angle)
                y = cy + radius * math.sin(angle)
                points.append((x, y))
            draw.polygon(points, fill=get_color())
    
    elif base_type == 5:
        for _ in range(int(columns * rows)):
            x, y = random.randint(0, width), random.randint(0, height)
            w, h = random.randint(30, 120), random.randint(10, 60)
            angle = random.choice([0, 45, 90, 135])
            if angle == 0:
                draw.rectangle([x, y, x+w, y+h], fill=get_color())
            else:
                import math
                rad = math.radians(angle)
                cos_a, sin_a = math.cos(rad), math.sin(rad)
                points = [
                    (x, y),
                    (x + w*cos_a, y + w*sin_a),
                    (x + w*cos_a - h*sin_a, y + w*sin_a + h*cos_a),
                    (x - h*sin_a, y + h*cos_a)
                ]
                draw.polygon(points, fill=get_color())
    
    elif base_type == 6:
        import math
        num_stars = random.randint(15, 30)
        for _ in range(num_stars):
            cx, cy = random.randint(0, width), random.randint(0, height)
            num_points = random.choice([4, 5, 6, 7, 8])
            outer_r = random.randint(20, 60)
            inner_r = outer_r * random.uniform(0.3, 0.6)
            points = []
            for i in range(num_points * 2):
                angle = math.pi * i / num_points
                r = outer_r if i % 2 == 0 else inner_r
                points.append((cx + r * math.cos(angle), cy + r * math.sin(angle)))
            draw.polygon(points, fill=get_color())
    
    elif base_type == 7:
        for x in range(0, width, int(x_incr*1.5)):
            for y in range(0, height, int(y_incr*1.5)):
                skew = random.randint(10, 40)
                w, h = x_incr*1.3, y_incr*1.3
                draw.polygon([(x,y), (x+w,y+skew), (x+w,y+h+skew), (x,y+h)], fill=get_color())
    
    elif base_type == 8:
        for _ in range(int(columns * rows * 0.7)):
            x, y = random.randint(0, width-100), random.randint(0, height-100)
            size = random.randint(40, 100)
            thickness = random.randint(15, 40)
            orientation = random.randint(0, 3)
            if orientation == 0:
                draw.polygon([(x,y), (x+thickness,y), (x+thickness,y+size-thickness), 
                             (x+size,y+size-thickness), (x+size,y+size), (x,y+size)], fill=get_color())
            elif orientation == 1:
                draw.polygon([(x,y), (x+size,y), (x+size,y+thickness), (x+thickness,y+thickness),
                             (x+thickness,y+size), (x,y+size)], fill=get_color())
            elif orientation == 2:
                draw.polygon([(x,y), (x+size,y), (x+size,y+size), (x+size-thickness,y+size),
                             (x+size-thickness,y+thickness), (x,y+thickness)], fill=get_color())
            else:
                draw.polygon([(x,y+thickness), (x+size-thickness,y+thickness), (x+size-thickness,y),
                             (x+size,y), (x+size,y+size), (x,y+size)], fill=get_color())
    
    elif base_type == 9:
        for _ in range(int(columns * rows * 0.8)):
            x, y = random.randint(0, width-100), random.randint(0, height-100)
            w1, w2 = random.randint(40, 120), random.randint(40, 120)
            h = random.randint(30, 100)
            draw.polygon([(x, y), (x+w1, y), (x+w2, y+h), (x-(w1-w2), y+h)], fill=get_color())
    
    elif base_type == 10:
        for x in range(0, width, int(x_incr*2)):
            for y in range(0, height, int(y_incr*2)):
                size = random.uniform(x_incr*0.8, x_incr*1.5)
                thickness = size * random.uniform(0.2, 0.4)
                cx, cy = x + random.randint(-20, 20), y + random.randint(-20, 20)
                draw.rectangle([cx-size/2, cy-thickness/2, cx+size/2, cy+thickness/2], fill=get_color())
                draw.rectangle([cx-thickness/2, cy-size/2, cx+thickness/2, cy+size/2], fill=get_color())
    
    else:
        import math
        for _ in range(int(columns * rows * 0.5)):
            cx, cy = random.randint(0, width), random.randint(0, height)
            radius = random.randint(25, 70)
            rotation = random.uniform(0, math.pi/3)
            points = []
            for i in range(6):
                angle = rotation + math.pi / 3 * i
                x = cx + radius * math.cos(angle)
                y = cy + radius * math.sin(angle)
                points.append((x, y))
            draw.polygon(points, fill=get_color())
    
    for i in range(random.randint(30, 60)):
        shape_type = random.choice(['triangle', 'rect', 'diamond', 'line'])
        if random.random() < 0.1:
            shape_type = 'line'
        
        overlay_color = random.choice(color_palette)
        if random.random() < 0.12:
            overlay_color = random.choice(accents)
        
        if shape_type == 'triangle':
            x1, y1 = random.randint(0, width), random.randint(0, height)
            x2, y2 = x1 + random.randint(-60, 60), y1 + random.randint(-60, 60)
            x3, y3 = x1 + random.randint(-60, 60), y1 + random.randint(-60, 60)
            draw.polygon([(x1,y1), (x2,y2), (x3,y3)], fill=overlay_color+(180,))
        
        elif shape_type == 'rect':
            x, y = random.randint(0, width-50), random.randint(0, height-50)
            w, h = random.randint(20, 80), random.randint(20, 80)
            draw.rectangle([x, y, x+w, y+h], fill=overlay_color+(160,))
        
        elif shape_type == 'diamond':
            cx, cy = random.randint(30, width-30), random.randint(30, height-30)
            size = random.randint(20, 50)
            draw.polygon([(cx,cy-size), (cx+size,cy), (cx,cy+size), (cx-size,cy)], fill=overlay_color+(170,))
        
        elif shape_type == 'line':
            x1, y1 = random.randint(0, width), random.randint(0, height)
            x2, y2 = random.randint(0, width), random.randint(0, height)
            draw.line([(x1,y1), (x2,y2)], fill=overlay_color, width=random.randint(3, 8))
    
    return img

num_patterns = 50
print(f"Generating {num_patterns} patterns...")

for i in range(num_patterns):
    palette = color_palettes[i % len(color_palettes)]
    img = draw_pattern(i, palette, seed=i*137 + 42)
    filename = f"pattern_{i+1:02d}.png"
    filepath = os.path.join(output_dir, filename)
    img.save(filepath)
    print(f"Created {filename}")

print(f"Done! Saved to {output_dir}")
