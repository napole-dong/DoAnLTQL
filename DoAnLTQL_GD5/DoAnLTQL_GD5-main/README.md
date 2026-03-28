

1. Giải pháp Container: Hướng dẫn cách dùng TableLayoutPanel (để chia lưới theo %) và SplitContainer thay vì kéo thả tự do.
2. Thuộc tính Neo/Đổ đầy: Quy tắc thiết lập thuộc tính `Dock` (Fill, Top...) và `Anchor` (Top, Bottom, Left, Right) cho các Control (DataGridView, Button, GroupBox, Panel) để chúng tự động co giãn theo container cha.
3. Chống vỡ Form: Cấu hình chuẩn cho Form chính (Main Form) và các Form con về các thuộc tính: `AutoScaleMode` (nên dùng Dpi hay Font?), `MinimumSize` (để không bị co lại quá nhỏ làm mất control), và `StartPosition`.
4. Code Helper (Tùy chọn): Viết cho tôi một class tiện ích (Utility Class) tĩnh, chứa một hàm truyền vào một Form. Hàm này sẽ đệ quy quét tất cả các Control trên Form đó và tự động tinh chỉnh kích thước/font chữ sao cho đồng bộ nhất.

Mục tiêu: Bỏ hoàn toàn việc fix cứng tọa độ (Location) và kích thước (Size) bằng số. Giao diện phải co giãn mượt mà.