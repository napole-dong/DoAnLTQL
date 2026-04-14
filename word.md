# [GD1]

## 1. Những điều đạt được

Trong giai đoạn GD1, nhóm đã hoàn thành đúng mục tiêu nền tảng của một hệ thống POS theo hướng khởi tạo kiến trúc dữ liệu và khung ứng dụng. Ở mức triển khai, dự án đã khởi tạo được ứng dụng WinForms chạy ổn định với điểm vào chương trình rõ ràng, đồng thời xây dựng đầy đủ các thực thể cốt lõi cho nghiệp vụ quán cà phê gồm bàn, loại món, món, nhân viên, khách hàng, hóa đơn và chi tiết hóa đơn. Song song đó, nhóm đã áp dụng công nghệ Entity Framework Core theo hướng Code First, cấu hình kết nối SQL Server qua tệp cấu hình và tạo migration khởi tạo cơ sở dữ liệu với các khóa chính, khóa ngoại và chỉ mục cần thiết. Kết quả này cho thấy hệ thống ở GD1 đã đáp ứng yêu cầu giai đoạn: hình thành được mô hình dữ liệu nghiệp vụ nhất quán, có khả năng tạo và quản lý schema dữ liệu, và sẵn sàng làm nền cho các chức năng nghiệp vụ ở các giai đoạn tiếp theo.

## 2. Khó khăn và giải pháp

Khó khăn chính trong GD1 tập trung ở bài toán thiết kế mô hình dữ liệu sao cho vừa phản ánh đúng nghiệp vụ thực tế của quán cà phê, vừa đảm bảo khả năng ánh xạ quan hệ trong Entity Framework Core. Việc xác định quan hệ giữa hóa đơn, chi tiết hóa đơn, bàn, nhân viên và khách hàng đòi hỏi thống nhất logic nghiệp vụ từ sớm để tránh xung đột khi sinh migration. Bên cạnh đó, quá trình cấu hình kết nối cơ sở dữ liệu và đồng bộ schema ban đầu cũng phát sinh các vấn đề kỹ thuật thường gặp như khác biệt cấu hình môi trường và ràng buộc dữ liệu giữa các bảng. Để xử lý, nhóm đã chuẩn hóa lại các thực thể và khóa liên kết theo hướng rõ nghĩa, sử dụng migration để kiểm soát phiên bản cơ sở dữ liệu, thực hiện debug và kiểm thử build nhằm bảo đảm ứng dụng khởi chạy ổn định, đồng thời giữ cấu trúc mã ở mức gọn cho giai đoạn đầu để thuận lợi refactor và tách lớp ở các giai đoạn sau. Cách tiếp cận này giúp giảm rủi ro sai lệch dữ liệu ngay từ nền móng và tạo được trục kỹ thuật ổn định cho việc mở rộng module.

## 3. Những điều chưa làm được

Do phạm vi GD1 ưu tiên phần móng kỹ thuật, một số nội dung quan trọng vẫn chưa được hoàn thiện trong giai đoạn này. Ở lớp chức năng, hệ thống mới dừng ở khung giao diện cơ bản, chưa triển khai đầy đủ các màn hình nghiệp vụ và luồng thao tác bán hàng thực tế; ở lớp trải nghiệm, UI/UX chưa được tối ưu cho vận hành tại quầy. Về xử lý nghiệp vụ, nhiều tình huống phát sinh trong quá trình phục vụ và thanh toán chưa được hiện thực thành logic chi tiết; đồng thời cấu trúc hiện tại chưa mở rộng sang các module chức năng cao hơn như quản trị nghiệp vụ theo lớp dịch vụ hoàn chỉnh. Những hạn chế này là phù hợp với đặc thù của giai đoạn khởi tạo, và cũng là cơ sở rõ ràng để định hướng ưu tiên phát triển ở các mốc tiếp theo.

# [GD2]

## 1. Những điều đạt được

Ở giai đoạn GD2, dự án đã chuyển từ mức khởi tạo nền tảng sang mức hình thành các module chức năng có thể thao tác trực tiếp trên dữ liệu. Nhóm đã triển khai màn hình Dashboard làm trung tâm vận hành với các chỉ số tổng quan theo ngày như doanh thu, số hóa đơn, trạng thái sử dụng bàn, tổng số món đã bán, đồng thời bổ sung biểu đồ doanh thu 7 ngày, danh sách hóa đơn gần đây và danh sách hoạt động gần nhất. Song song đó, module quản lý bàn đã được hoàn thiện ở mức nghiệp vụ cơ bản với sơ đồ bàn động theo dữ liệu, chức năng thêm và xóa bàn có kiểm tra ràng buộc, cùng xử lý chuyển bàn hoặc gộp bàn trên hóa đơn đang phục vụ. Module quản lý món cũng đã được hiện thực với các thao tác thêm, cập nhật, xóa, lọc theo loại, tìm kiếm và xuất dữ liệu ra CSV. Về công nghệ, nhóm tiếp tục vận dụng WinForms kết hợp Entity Framework Core và SQL Server, sử dụng truy vấn LINQ cùng các xử lý AsNoTracking ở các luồng đọc dữ liệu để giữ hiệu năng phù hợp với quy mô giai đoạn. Nhìn tổng thể, hệ thống ở GD2 đã đáp ứng đúng mục tiêu mở rộng chức năng quản trị cơ bản trên nền dữ liệu đã xây dựng từ GD1.

## 2. Khó khăn và giải pháp

Khó khăn nổi bật của GD2 nằm ở việc đồng bộ dữ liệu và trạng thái giữa nhiều thực thể khi bắt đầu có thao tác nghiệp vụ phức hợp. Với quản lý bàn, bài toán không chỉ dừng ở hiển thị danh sách mà còn phải bảo đảm tính nhất quán giữa trạng thái bàn, hóa đơn nguồn, hóa đơn đích và chi tiết món khi thực hiện chuyển bàn hoặc gộp bàn. Với dashboard, việc tổng hợp doanh thu và thống kê theo thời gian yêu cầu truy vấn gom nhóm trên dữ liệu hóa đơn và chi tiết hóa đơn sao cho vừa đúng nghiệp vụ vừa không làm gián đoạn trải nghiệm giao diện. Để xử lý, nhóm đã triển khai mô hình sơ đồ bàn động bằng cách sinh điều khiển giao diện từ dữ liệu thực, áp dụng các điều kiện kiểm tra trước khi thao tác ghi dữ liệu như kiểm tra bàn trống, kiểm tra hóa đơn đang mở và kiểm tra phát sinh ràng buộc trước khi xóa. Ở phần thống kê, nhóm chuẩn hóa truy vấn theo mốc thời gian, kết hợp cơ chế fallback an toàn khi phát sinh lỗi để tránh làm vỡ màn hình tổng quan. Cách giải quyết này giúp hệ thống duy trì được tính ổn định vận hành trong bối cảnh chức năng bắt đầu tăng nhanh về độ phức tạp.

## 3. Những điều chưa làm được

Mặc dù đã mở rộng tốt về chức năng quản trị, GD2 vẫn còn một số giới hạn cần tiếp tục xử lý ở các giai đoạn sau để đạt mức hoàn thiện cao hơn. Trước hết, hệ thống chưa có luồng xác thực và phân quyền vận hành đầy đủ, do đó việc kiểm soát vai trò người dùng chưa được hiện thực trọn vẹn ở tầng chức năng. Kế đến, một số nghiệp vụ mới ở mức cơ bản như trạng thái món đang được hiển thị theo hướng mặc định, chức năng nhập dữ liệu món chưa đi vào luồng import thực sự, và các thao tác cập nhật nhiều bước như chuyển hoặc gộp bàn chưa được bao bọc bởi giao dịch dữ liệu để tăng mức an toàn khi có lỗi trung gian. Ngoài ra, kiến trúc hiện tại vẫn nghiêng về xử lý trực tiếp giữa giao diện và DbContext, phù hợp cho giai đoạn phát triển nhanh nhưng chưa tối ưu cho mở rộng lớn và kiểm thử tự động. Các điểm này không làm mất giá trị kết quả GD2, nhưng là phạm vi nâng cấp cần thiết để hệ thống tiến tới tính sẵn sàng triển khai cao hơn.

# [GD3]

## 1. Những điều đạt được

Trong GD3, nhóm đã tiếp tục hoàn thiện lớp giao diện và mở rộng rõ hơn năng lực vận hành ở mức quản trị nội bộ. Ứng dụng vẫn chạy từ Dashboard trung tâm, đồng thời duy trì các module đã có ở GD2 như quản lý bàn và quản lý món với các thao tác thực tế trên dữ liệu. Bên cạnh đó, giai đoạn này bổ sung thêm màn hình loại món, thể hiện định hướng chuẩn hóa danh mục phục vụ các bước phát triển sau. Ở góc nhìn kỹ thuật, hệ thống tiếp tục bám theo hướng WinForms kết hợp Entity Framework Core và SQL Server, giữ vững mô hình dữ liệu cốt lõi đã khởi tạo từ GD1. Nhìn chung, GD3 đã đạt mục tiêu tăng độ đầy đặn của phần giao diện và chức năng quản trị, tạo tiền đề để tái cấu trúc kiến trúc ở giai đoạn kế tiếp.

## 2. Khó khăn và giải pháp

Khó khăn chính của GD3 nằm ở việc mở rộng chức năng nhanh trong khi kiến trúc vẫn chủ yếu đặt logic trực tiếp tại tầng form. Khi số thao tác quản trị tăng lên, đặc biệt ở các luồng liên quan bàn, món và thống kê tổng quan, yêu cầu đồng bộ trạng thái dữ liệu và phản hồi giao diện trở nên phức tạp hơn so với GD2. Nhóm đã xử lý theo hướng ưu tiên ổn định tác nghiệp: giữ truy vấn và kiểm tra điều kiện nghiệp vụ ở mức đủ chặt để hạn chế lỗi dữ liệu, đồng thời chuẩn hóa lại cách tổ chức giao diện theo vai trò dashboard trung tâm. Việc bổ sung khung màn hình loại món trong giai đoạn này cũng là một quyết định kỹ thuật hợp lý, vì giúp tách dần từng nhóm nghiệp vụ để chuẩn bị chuyển sang mô hình phân lớp rõ ràng hơn ở GD4.

## 3. Những điều chưa làm được

Hạn chế nổi bật của GD3 là một số phần mở rộng mới chưa đạt mức hoàn thiện đồng đều với các module đã ổn định trước đó. Cụ thể, màn hình loại món mới dừng nhiều ở khung giao diện, chưa thể hiện đầy đủ chu trình nghiệp vụ như các module quản trị khác; đồng thời kiến trúc chưa tách lớp BUS, DAL, DTO nên khả năng tái sử dụng và kiểm thử còn hạn chế. Bên cạnh đó, hệ thống vẫn chưa đưa vào luồng đăng nhập và phân quyền vận hành xuyên suốt ở mức toàn ứng dụng. Đây là các giới hạn mang tính kỹ thuật chuyển tiếp, phù hợp với đặc thù của một giai đoạn đang ưu tiên mở rộng bề mặt chức năng trước khi tái cấu trúc sâu.

# [GD4]

## 1. Những điều đạt được

GD4 đánh dấu bước chuyển kiến trúc quan trọng của dự án khi nhóm đã tái tổ chức mã nguồn theo mô hình phân lớp gồm Forms, BUS, DAL và DTO thay vì xử lý tập trung ở code-behind. Cùng với đó, hệ thống được mở rộng thêm các module quản lý khách hàng và quản lý nhân viên, giúp phạm vi quản trị dữ liệu nội bộ trở nên đầy đủ hơn. Ở lớp dữ liệu, giai đoạn này bổ sung bảng tài khoản và vai trò, đồng thời thực hiện các migration chuẩn hóa quan hệ đăng nhập và phân quyền theo hướng có cấu trúc. Kết quả đạt được không chỉ nằm ở số lượng chức năng, mà còn ở việc định hình một nền kiến trúc dễ bảo trì và dễ mở rộng hơn cho các giai đoạn có nghiệp vụ phức tạp. Như vậy, GD4 đã hoàn thành tốt mục tiêu vừa mở rộng module vừa nâng cấp chất lượng kỹ thuật nền.

## 2. Khó khăn và giải pháp

Khó khăn lớn nhất ở GD4 là bài toán chuyển đổi từ cách phát triển nhanh sang cách tổ chức có kiến trúc, trong khi vẫn phải giữ tính liên tục của các chức năng đã chạy được. Việc tách lớp đòi hỏi nhóm xử lý lại luồng gọi dữ liệu, chuẩn hóa DTO trung gian và hạn chế phụ thuộc trực tiếp từ giao diện xuống DbContext. Đồng thời, quá trình chuẩn hóa tài khoản và vai trò cũng yêu cầu thao tác migration cẩn trọng để tránh phá vỡ tính toàn vẹn dữ liệu hiện có. Giải pháp được áp dụng là triển khai tách lớp theo từng module, kết hợp migration theo bước nhỏ và bổ sung guard nghiệp vụ tại BUS trước khi ghi dữ liệu. Cách làm này giúp giảm rủi ro khi chuyển đổi kiến trúc và duy trì được sự ổn định của hệ thống trong giai đoạn tái cấu trúc.

## 3. Những điều chưa làm được

Dù đã có tiến bộ kiến trúc rõ rệt, GD4 vẫn còn các điểm chưa đạt mức vận hành hoàn chỉnh. Hệ thống chưa bắt buộc đi qua luồng đăng nhập khi khởi động toàn ứng dụng, nên phân quyền mới ở mức dữ liệu và quy tắc xử lý cục bộ, chưa thành kiểm soát phiên làm việc tổng thể. Một số thành phần chức năng vẫn còn ở dạng điều hướng hoặc placeholder, chưa tạo thành chuỗi nghiệp vụ liên tục từ đầu đến cuối. Ngoài ra, các yêu cầu bảo mật nâng cao như chính sách mật khẩu mạnh và kiểm soát thao tác theo ngữ cảnh người dùng vẫn cần được hoàn thiện thêm ở các giai đoạn sau.

# [GD5]

## 1. Những điều đạt được

Tại GD5, phạm vi dự án được mở rộng mạnh về bề mặt module, thể hiện qua việc bổ sung các màn hình bán hàng, hóa đơn và quản lý kho bên cạnh các module quản trị đã ổn định ở GD4. Ở tầng dữ liệu, hệ thống thêm thực thể nguyên liệu và các cập nhật migration liên quan trạng thái món cùng mô tả loại món, tạo nền cho kiểm soát vận hành chi tiết hơn. Nhóm cũng tiếp tục duy trì cấu trúc phân lớp BUS, DAL, DTO trong các chức năng quản trị hiện hữu, giúp mã nguồn không quay lại trạng thái phụ thuộc trực tiếp như giai đoạn đầu. Có thể xem GD5 là giai đoạn mở rộng biên chức năng quan trọng, chuẩn bị không gian cho POS lõi và quản trị kho phát triển sâu ở các mốc tiếp theo. Giá trị chính của giai đoạn này là hoàn thiện khung hệ thống đa module hơn là hoàn tất ngay toàn bộ luồng nghiệp vụ mới.

## 2. Khó khăn và giải pháp

Khó khăn đặc trưng ở GD5 đến từ việc mở rộng đồng thời nhiều hướng nghiệp vụ khi nền logic POS lõi vẫn đang trong quá trình hoàn thiện. Việc đưa thêm kho nguyên liệu và màn hình bán hàng làm tăng đáng kể độ phức tạp về đồng bộ dữ liệu và tổ chức điều hướng giữa các module. Nhóm đã chọn hướng giải quyết theo từng lớp ưu tiên: giữ ổn định các module CRUD quản trị đã chắc, đồng thời thiết kế trước khung giao diện và cấu trúc dữ liệu cho các module mới để giảm chi phí thay đổi ở giai đoạn sau. Cách tiếp cận này thể hiện tư duy phát triển theo lộ trình, tránh dàn trải triển khai nghiệp vụ sâu khi nền dữ liệu và điều phối hệ thống chưa đủ chín.

## 3. Những điều chưa làm được

Hạn chế lớn của GD5 là các module mới mở rộng chưa đồng đều về mức hoàn thiện so với nhóm quản trị dữ liệu truyền thống. Màn hình bán hàng và hóa đơn mới ở mức khung, chưa thể hiện đầy đủ luồng tác nghiệp POS xuyên suốt; trong khi đó cơ chế xác thực và kiểm soát quyền truy cập vẫn chưa được thực thi nhất quán ở cấp ứng dụng. Một số hạng mục kho nguyên liệu dù đã xuất hiện ở lớp dữ liệu nhưng chưa hoàn thiện trọn vẹn ở mức migration và nghiệp vụ vận hành. Vì vậy, GD5 phù hợp được đánh giá là bản mở rộng kiến trúc chức năng, chưa phải bản hoàn thiện nghiệp vụ lõi.

# [GD6]

## 1. Những điều đạt được

GD6 là bước tiến rõ rệt về tính sử dụng thực tế khi hệ thống đã tích hợp luồng đăng nhập và bắt đầu chạy ứng dụng theo phiên người dùng, thay vì vào thẳng màn hình vận hành như các giai đoạn trước. Cùng với đó, các lớp BUS và DAL cho bán hàng, hóa đơn và đăng nhập được bổ sung, giúp nhiều nghiệp vụ POS cốt lõi có thể thao tác được ở mức đầu-cuối. Nhóm cũng triển khai xử lý mật khẩu theo hướng băm, đồng thời hỗ trợ chuyển đổi mềm dữ liệu cũ, thể hiện sự quan tâm đến an toàn vận hành ngay trong quá trình nâng cấp. So với GD5, chất lượng triển khai của GD6 không chỉ là tăng số module mà còn nâng độ chín của luồng nghiệp vụ chính. Có thể xem đây là giai đoạn chuyển từ mở rộng khung chức năng sang hiện thực hóa dòng nghiệp vụ vận hành nội bộ.

## 2. Khó khăn và giải pháp

Thách thức của GD6 nằm ở việc kết hợp cơ chế đăng nhập mới với các module vốn phát triển theo kiểu độc lập ở giai đoạn trước. Khi đưa người dùng và phiên đăng nhập vào hệ thống, nhóm phải đồng thời xử lý bài toán xác thực, giữ trạng thái thao tác và duy trì điều hướng xuyên form mà không làm gãy các luồng nghiệp vụ đã có. Ở lớp nghiệp vụ, các thao tác nhiều bước như thanh toán, chuyển hoặc gộp bàn cũng đặt ra yêu cầu kiểm soát nhất quán dữ liệu cao hơn. Giải pháp được áp dụng là bổ sung BUS và DAL chuyên trách cho từng khối nghiệp vụ chính, tăng validate ở điểm vào và chuẩn hóa dần dữ liệu tài khoản theo vai trò. Nhờ đó, hệ thống đạt được mức ổn định đủ cho demo nghiệp vụ thực tế, dù vẫn còn các điểm cần hardening thêm.

## 3. Những điều chưa làm được

GD6 vẫn còn khoảng trống giữa việc có đăng nhập và việc thực thi phân quyền đầy đủ trên toàn bộ ứng dụng. Dù phiên đăng nhập đã xuất hiện, nhiều luồng điều hướng và kiểm soát thao tác chưa đồng nhất giữa các form, làm giảm tính nhất quán trong trải nghiệm và bảo mật chức năng. Ngoài ra, hệ thống còn tồn tại một số điểm nghẽn hiệu năng do cơ chế làm mới dữ liệu theo chu kỳ ngắn và các thao tác cơ sở dữ liệu chưa được tối ưu toàn diện cho tải lớn. Các hạn chế này không phủ nhận giá trị tiến bộ của GD6, nhưng cho thấy nhu cầu cấp thiết của một giai đoạn hardening nghiệp vụ và vận hành sâu hơn.

# [GD7]

## 1. Những điều đạt được

Trong GD7, dự án chuyển mạnh sang hướng hardening với trọng tâm là phân quyền, an toàn vận hành và chuẩn hóa dữ liệu nghiệp vụ. Hệ thống đã bổ sung các lớp quản lý quyền và tài khoản chuyên biệt, mở rộng mô hình dữ liệu với permission, công thức món và phiếu nhập xuất kho, đồng thời tăng số migration phục vụ ràng buộc và chỉ mục theo hướng chặt chẽ hơn. Ở mức vận hành, cơ chế đăng nhập, đăng xuất và quay lại màn hình xác thực được tổ chức theo vòng đời ứng dụng rõ ràng hơn, đi kèm nền tảng logging và xử lý lỗi có cấu trúc. Việc đồng bộ quyền mặc định theo mẫu vai trò cũng giúp giảm rủi ro lệch quyền giữa dữ liệu cũ và dữ liệu mới. Nhìn tổng thể, GD7 đã đạt mục tiêu nâng chuẩn kỹ thuật và quản trị truy cập thay vì chỉ mở rộng giao diện.

## 2. Khó khăn và giải pháp

Khó khăn chính của GD7 là đảm bảo tính nhất quán quyền thao tác trong khi dữ liệu và luồng nghiệp vụ đã phát triển qua nhiều giai đoạn. Khi chuyển từ cách kiểm soát quyền đơn giản sang ma trận quyền theo feature và action, nhóm phải xử lý đồng thời cả lớp dữ liệu, lớp nghiệp vụ và lớp giao diện để tránh mâu thuẫn hành vi. Giải pháp được áp dụng là chuẩn hóa bộ quyền theo template, đồng bộ quyền mặc định ở startup, đồng thời đưa các kiểm tra quyền vào BUS và gate hiển thị chức năng ở UI. Song song đó, nhóm tăng cường nền tảng theo dõi vận hành bằng logging có mã lỗi và correlation, giúp việc truy vết sự cố rõ ràng hơn. Cách xử lý này phù hợp với mục tiêu hardening, vì tập trung giải quyết sai lệch hệ thống ở cấp kiến trúc thay vì vá cục bộ từng màn hình.

## 3. Những điều chưa làm được

Dù đã cải thiện đáng kể về quản trị quyền, GD7 vẫn còn các điểm cần hoàn thiện để đạt mức sản xuất. Một số chính sách bảo mật tài khoản và chuẩn mật khẩu chưa thật sự chặt ở mức doanh nghiệp, trong khi kiểm thử tự động cho các luồng rủi ro cao vẫn chưa được phủ đầy đủ. Bên cạnh đó, sự pha trộn giữa một số logic quyền theo vai trò cứng và quyền theo ma trận dữ liệu vẫn tạo nguy cơ thiếu đồng nhất ở các tình huống biên. Vì vậy, GD7 có thể được xem là bản hardening quan trọng, nhưng chưa phải điểm kết thúc cho yêu cầu bảo mật và chất lượng vận hành.

# [GD8]

## 1. Những điều đạt được

GD8 tiếp tục nâng cấp hệ thống theo chiều sâu nghiệp vụ và độ tin cậy kỹ thuật. Dự án bổ sung thêm các module công thức món và thống kê, đồng thời tập trung hóa luồng xử lý đơn hàng trong một dịch vụ chuyên trách để kiểm soát tốt hơn các thao tác thêm, đổi, xóa món, thanh toán và hủy. Ở tầng dữ liệu và giao dịch, giai đoạn này tăng cường các cơ chế quan trọng như soft delete diện rộng, kiểm soát đồng thời bằng row version và chuẩn hóa transaction theo execution strategy. Hệ thống cũng có dự án kiểm thử đi kèm và đạt trạng thái build, test ổn định trong phạm vi giai đoạn. Tổng thể, GD8 đã đạt mục tiêu nâng cấp từ một bản vận hành nội bộ sang một bản có nền hardening rõ ràng về tính nhất quán dữ liệu.

## 2. Khó khăn và giải pháp

Khó khăn lớn của GD8 là xử lý đồng thời ba mục tiêu: duy trì luồng nghiệp vụ đang chạy, gia tăng độ an toàn dữ liệu và không làm suy giảm trải nghiệm sử dụng. Khi gom logic đơn hàng về một điểm xử lý trung tâm, nhóm phải rà soát kỹ các thao tác biên để tránh sai lệch tổng tiền và trạng thái hóa đơn. Đồng thời, việc đưa soft delete và cơ chế concurrency vào hệ thống đòi hỏi hiệu chỉnh lại nhiều truy vấn nghiệp vụ hiện có. Giải pháp của nhóm là mở rộng migration theo từng lớp hardening, bổ sung guard xử lý lỗi, và tăng kiểm thử cho các flow trọng yếu. Cách triển khai này cho phép hệ thống tiến lên đáng kể về độ tin cậy mà vẫn giữ được khả năng vận hành thực tế.

## 3. Những điều chưa làm được

Bên cạnh các kết quả tích cực, GD8 vẫn tồn tại một số hạn chế quan trọng về mức sẵn sàng sản xuất. Cụ thể, cơ chế bootstrap tài khoản mặc định và một số xử lý mật khẩu còn yếu ở giai đoạn này, tạo rủi ro bảo mật nếu triển khai trực tiếp ra môi trường thật. Ngoài ra, một số thao tác cập nhật chi tiết hóa đơn và tải dữ liệu thống kê vẫn có nguy cơ gây sai lệch hoặc chậm giao diện ở dữ liệu lớn nếu không tiếp tục tinh chỉnh. Những hạn chế này là cơ sở để GD9 và GD10 tập trung vào hardening cuối cùng thay vì mở thêm chức năng mới.

# [GD9]

## 1. Những điều đạt được

Ở GD9, dự án thể hiện rõ định hướng hoàn thiện vận hành với việc mở rộng module kiểm toán thao tác, bổ sung thành phần báo cáo và in ấn, đồng thời chuẩn hóa hơn nữa luồng giao dịch ở tầng DAL. Việc đưa vào transaction runner dùng chung theo execution strategy giúp các nghiệp vụ nhiều bước giữa bàn, hóa đơn, nguyên liệu và nhân sự được xử lý nhất quán hơn. Giai đoạn này cũng hoàn thiện thêm mô hình kho theo cấu trúc phiếu tổng và chi tiết, nâng mức toàn vẹn dữ liệu cho các nghiệp vụ nhập xuất. Về cấu trúc hệ thống, sự xuất hiện rõ nét của presenters và services cho thấy nỗ lực giảm phụ thuộc trực tiếp vào code-behind. Như vậy, GD9 đã đạt mục tiêu chuyển từ hardening theo điểm sang hardening theo khung kiến trúc vận hành.

## 2. Khó khăn và giải pháp

Thách thức ở GD9 chủ yếu đến từ việc hợp nhất nhiều chuẩn xử lý đã hình thành qua các giai đoạn trước thành một cơ chế thống nhất, đặc biệt ở transaction và phân quyền. Khi số lượng module tăng cao, việc mỗi nơi tự quản lý giao dịch hoặc tự diễn giải quyền truy cập dễ tạo ra sai khác khó kiểm soát. Nhóm đã giải quyết bằng cách đưa các luồng ghi dữ liệu về một helper transaction thống nhất, đồng thời tăng cường các ràng buộc dữ liệu và migration integrity để giảm sai số vận hành. Ở cấp giao diện, việc bổ sung module audit log và báo cáo cũng giúp phản ánh tốt hơn mục tiêu truy vết và quản trị. Cách làm này phù hợp với giai đoạn tiền hoàn thiện, nơi tính ổn định và khả năng kiểm chứng được đặt cao hơn tốc độ thêm tính năng.

## 3. Những điều chưa làm được

GD9 vẫn còn một số điểm cần xử lý trước khi có thể xem là phiên bản sẵn sàng sản xuất. Trọng yếu nhất là sự chưa đồng nhất tuyệt đối giữa một số nguồn kiểm soát quyền, khiến hành vi thực tế có thể khác nhau giữa các màn hình ở vài tình huống. Bên cạnh đó, một số đoạn xử lý đồng bộ trong luồng nghiệp vụ dữ liệu còn tồn tại, làm giảm dư địa hiệu năng và khả năng mở rộng khi tải tăng. Ngoài ra, bộ kiểm thử tự động chưa hiện diện đầy đủ trong cấu trúc giai đoạn này, nên mức bảo vệ hồi quy chưa đạt kỳ vọng cho một bản phát hành ổn định cao.

# [GD10]

## 1. Những điều đạt được

GD10 là giai đoạn hoàn thiện tập trung vào chất lượng vận hành và xử lý các rủi ro nghiêm trọng đã nhận diện từ các mốc trước. Nhóm đã loại bỏ phụ thuộc mật khẩu khởi tạo cứng trong startup, chuyển sang cơ chế đọc cấu hình môi trường phù hợp hơn với thực hành triển khai. Đồng thời, các xử lý nghiệp vụ đơn hàng đã được chỉnh theo hướng đồng bộ aggregate trước khi tính lại tổng tiền, qua đó giảm nguy cơ sai lệch số liệu hóa đơn. Ở module hóa đơn, cách tính tổng tiền cũng được thống nhất theo dữ liệu chi tiết để bảo đảm nhất quán giữa danh sách và thông tin tiêu đề. Ngoài ra, cơ chế in hóa đơn được siết điều kiện theo trạng thái đã thanh toán, giúp tránh sai quy trình tác nghiệp. Các kết quả này cho thấy GD10 đạt vai trò của một giai đoạn hardening cuối, tập trung nâng độ tin cậy thay vì mở thêm nhiều chức năng.

## 2. Khó khăn và giải pháp

Khó khăn của GD10 là sửa các lỗi có mức ảnh hưởng cao mà không làm phát sinh hồi quy trên hệ thống đã lớn và đa module. Những điểm như bootstrap tài khoản, tổng tiền hóa đơn hay điều kiện in chứng từ đều nằm ở các nút nghiệp vụ nhạy cảm, nên yêu cầu can thiệp cẩn trọng ở cả lớp startup, BUS và DAL. Nhóm đã xử lý theo nguyên tắc sửa vào nguyên nhân gốc: thay cấu hình cứng bằng nguồn cấu hình môi trường, đồng bộ thao tác trên aggregate trước khi tái tính toán, và đặt gate nghiệp vụ rõ ràng cho thao tác in. Cách tiếp cận này giúp giảm rủi ro lặp lại lỗi ở luồng thực tế, đồng thời giữ được tính liên tục của quy trình vận hành đã có. Đây là hướng xử lý phù hợp với mục tiêu chốt chất lượng ở giai đoạn cuối.

## 3. Những điều chưa làm được

Mặc dù đạt tiến bộ tốt về ổn định, GD10 vẫn còn một số giới hạn quan trọng cần tiếp tục hoàn thiện trước khi phát hành ở mức production đầy đủ. Chính sách kiểm tra độ mạnh mật khẩu hiện vẫn chưa đạt mức chặt mong muốn cho môi trường có yêu cầu bảo mật cao; một số thao tác thống kê còn chạy đồng bộ trên luồng giao diện nên có thể gây chậm ở dữ liệu lớn; và một vài rủi ro biên liên quan truy vấn lịch sử khi kết hợp soft delete vẫn cần thêm kiểm thử xác nhận. Ngoài ra, mức độ bao phủ kiểm thử tự động và độ cập nhật tài liệu phát hành vẫn chưa tương xứng với quy mô hệ thống ở cuối chu kỳ. Vì vậy, GD10 có thể được đánh giá là bản beta rất gần production, nhưng còn cần một vòng củng cố cuối về kiểm thử và chính sách bảo mật.
