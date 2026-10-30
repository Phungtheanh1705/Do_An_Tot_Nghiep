# BỘ 12 ICON "KHÁM PHÁ THEO DANH MỤC" - LOCALEXPLORE

## 1. THÔNG SỐ KỸ THUẬT THIẾT KẾ (DESIGN SPECIFICATION)
* **Phong cách:** Lucide Minimal Outline / Vector Line Icons.
* **Ngôn ngữ hình học:** Bo góc mềm mại (`stroke-linecap="round"`, `stroke-linejoin="round"`), đường nét tối giản, hiện đại.
* **Độ dày nét vẽ (Stroke Width):** `1.8px` (chuẩn Lucide).
* **Màu nét vẽ (Stroke Color):** Xanh rêu đậm `#285A45` (Forest Deep Moss Green).
* **Nền icon (Background Container):** Tròn 64px, màu xanh lá nhạt `#EAF1EA`, viền nhẹ `rgba(40, 90, 69, 0.12)`.
* **Kích thước hiển thị:** 28px × 28px trong viewBox `0 0 24 24`.
* **Nền SVG:** 100% trong suốt (Transparent background), hỗ trợ kế thừa `currentColor`.

---

## 2. DANH SÁCH MÃ NGUỒN SVG THUẦN (STANDALONE TRANSPARENT SVGS)

### 1. Food & Cuisine – Ẩm thực
```xml
<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
  <path d="M12 2v3"></path>
  <path d="M8 2v3"></path>
  <path d="M16 2v3"></path>
  <path d="M3 10c0 4.97 4.03 9 9 9s9-4.03 9-9H3z"></path>
  <line x1="8" y1="21" x2="16" y2="21"></line>
</svg>
```

### 2. Local Culture – Văn hóa bản địa
```xml
<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
  <line x1="3" y1="22" x2="21" y2="22"></line>
  <line x1="6" y1="18" x2="6" y2="11"></line>
  <line x1="10" y1="18" x2="10" y2="11"></line>
  <line x1="14" y1="18" x2="14" y2="11"></line>
  <line x1="18" y1="18" x2="18" y2="11"></line>
  <polygon points="12 2 20 7 4 7"></polygon>
</svg>
```

### 3. Traditional Crafts – Làng nghề truyền thống
```xml
<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
  <path d="M10 2h4"></path>
  <path d="M10 2v3c0 2-4 3-4 7 0 4.4 2.7 8 6 8s6-3.6 6-8c0-4-4-5-4-7V2"></path>
  <path d="M6 9a3 3 0 0 0-3 3c0 2.5 1.5 4 3 4"></path>
  <path d="M18 9a3 3 0 0 1 3 3c0 2.5-1.5 4-3 4"></path>
  <path d="M9 22h6"></path>
</svg>
```

### 4. Mountains – Núi rừng
```xml
<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
  <path d="m8 3 4 8 5-5 5 15H2L8 3z"></path>
  <path d="M4.15 15.1c2.62-1.6 5.25-1.4 7.85.4 2.75 2 5.5 2 8.25.2"></path>
</svg>
```

### 5. Agriculture – Nông nghiệp
```xml
<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
  <path d="m2 22 10-10"></path>
  <path d="M16 8a4 4 0 0 0-4-4c-2 2-2 4 0 6 2 2 4 2 4-2Z"></path>
  <path d="M8 16a4 4 0 0 0 4 4c2-2 2-4 0-6-2-2-4-2-4 2Z"></path>
  <path d="M14 14a4 4 0 0 0 0-6c-2 0-4 2-2 4 2 2 2 2 2 2Z"></path>
  <path d="M10 10a4 4 0 0 0 0 6c2 0 4-2 2-4-2-2-2-2-2-2Z"></path>
</svg>
```

### 6. Beach & Islands – Biển đảo
```xml
<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
  <path d="M12 4V2"></path>
  <path d="m4.93 6.93 1.41 1.41"></path>
  <path d="M2 12h2"></path>
  <path d="M20 12h2"></path>
  <path d="m17.66 8.34 1.41-1.41"></path>
  <path d="M16 12a4 4 0 0 0-8 0"></path>
  <path d="M2 17c1.5-.8 3.5-.8 5 0 1.5.8 3.5.8 5 0 1.5-.8 3.5-.8 5 0 1.5.8 3.5.8 5 0"></path>
  <path d="M2 21c1.5-.8 3.5-.8 5 0 1.5.8 3.5.8 5 0 1.5-.8 3.5-.8 5 0 1.5.8 3.5.8 5 0"></path>
</svg>
```

### 7. Adventure – Phiêu lưu
```xml
<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
  <circle cx="12" cy="12" r="10"></circle>
  <polygon points="16.24 7.76 14.12 14.12 7.76 16.24 9.88 9.88 16.24 7.76"></polygon>
</svg>
```

### 8. Workshop – Workshop thủ công
```xml
<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
  <circle cx="13.5" cy="6.5" r=".8" fill="#285A45"></circle>
  <circle cx="17.5" cy="10.5" r=".8" fill="#285A45"></circle>
  <circle cx="8.5" cy="7.5" r=".8" fill="#285A45"></circle>
  <circle cx="6.5" cy="12.5" r=".8" fill="#285A45"></circle>
  <path d="M12 2C6.5 2 2 6.5 2 12s4.5 10 10 10c.926 0 1.648-.746 1.648-1.688 0-.437-.18-.835-.437-1.125-.29-.289-.438-.652-.438-1.125a1.64 1.64 0 0 1 1.668-1.668h1.996c3.051 0 5.555-2.503 5.555-5.554C21.965 6.012 17.461 2 12 2z"></path>
</svg>
```

### 9. Camping – Cắm trại
```xml
<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
  <path d="M3.5 21 14 3"></path>
  <path d="m20.5 21-10.5-18"></path>
  <path d="M15.5 21 12 15l-3.5 6"></path>
  <path d="M2 21h20"></path>
</svg>
```

### 10. Photography – Nhiếp ảnh
```xml
<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
  <path d="M14.5 4h-5L7 7H4a2 2 0 0 0-2 2v9a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2V9a2 2 0 0 0-2-2h-3l-2.5-3z"></path>
  <circle cx="12" cy="13" r="3"></circle>
</svg>
```

### 11. Nature – Thiên nhiên
```xml
<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
  <path d="M11 20A7 7 0 0 1 9.8 6.1C15.5 5 17 4.48 19 2c1 2 2 4.18 2 8 0 5.5-4.78 10-10 10Z"></path>
  <path d="M2 21c0-3 1.85-5.36 5.08-6C9.5 14.52 12 13 13 12"></path>
</svg>
```

### 12. Community – Trải nghiệm cộng đồng
```xml
<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
  <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"></path>
  <circle cx="9" cy="7" r="4"></circle>
  <path d="M22 21v-2a4 4 0 0 0-3-3.87"></path>
  <path d="M16 3.13a4 4 0 0 1 0 7.75"></path>
</svg>
```
