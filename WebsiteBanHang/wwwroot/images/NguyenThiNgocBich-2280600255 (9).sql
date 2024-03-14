----1. Thêm một dòng mới vào bảng SINHVIEN với giá trị:

----190001 Đào Thị Tuyết Hoa 08/03/2001 0 19DTH02
INSERT INTO SINHVIEN 
VALUES ('190001',N'Đào Thị Tuyết Hoa',' 08/03/2001','0','19DTH02')
SELECT *FROM SINHVIEN

----2. Hãy đổi tên môn học ‘Lý thuyết đồ thị’ thành ‘Toán rời rạc’.
UPDATE MONHOC
SET TENMH =  ' Lý thuyết đồ thị'
WHERE TENMH = 'Toán rời rạc'
SELECT *FROM MONHOC
----3. Hiển thị tên các môn học không có thực hành.
SELECT TENMH
FROM MONHOC 
WHERE TCTH =0
----4. Hiển thị tên các môn học vừa có lý thuyết, vừa có thực hành.
SELECT TENMH
FROM MONHOC 
WHERE TCLT >0 AND TCTH >0
----5. In ra tên các môn học có ký tự đầu của tên là chữ ‘C’.
SELECT TENMH
FROM MONHOC 
WHERE TENMH LIKE 'C%';
----6. Liệt kê thông tin những sinh viên mà họ chứa chữ ‘Thị’.
SELECT *FROM SINHVIEN 
WHERE HOTEN LIKE N'%Thị%';
----7. In ra 2 lớp có sĩ số đông nhất (bằng nhiều cách). Hiển thị: Mã lớp, Tên lớp, Sĩ số. Nhận xét?
SELECT TOP 2 *
FROM LOP 
ORDER BY SISO DESC
----8. In danh sách SV theo từng lớp: MSSV, Họ tên SV, Năm sinh, Phái (Nam/Nữ).
SELECT T.MALOP,T.MSSV,T.HOTEN,T.NTNS,T.PHAI
FROM LOP T1,SINHVIEN T
WHERE T1.MALOP = T.MALOP

----9. Cho biết những sinh viên có tuổi ≥ 20, thông tin gồm: Họ tên sinh viên, Ngày sinh, Tuổi. 
SELECT HOTEN, NTNS 
FROM SINHVIEN 
WHERE DATEDIFF(YY,NTNS,GETDATE())>=20
----10. Liệt kê tên các môn học SV đã dự thi nhưng chưa có điểm.
SELECT T1.MAMH,T1.TENMH
FROM MONHOC T1,DIEMSV T
WHERE T1.MAMH= T.MAMH AND T.DIEM IS NULL
----11. Liệt kê kết quả học tập của SV có mã số 170001. Hiển thị: MSSV, HoTen, TenMH, Diem.
SELECT T2.MSSV,HOTEN, TENMH,DIEM
FROM MONHOC T1, SINHVIEN T2, DIEMSV T3
WHERE  T2.MSSV = 170001 and T3.MSSV = 170001 and T3.MAMH = T1.MAMH
----12. Liệt kê tên sinh viên và mã môn học mà sv đó đăng ký với điểm trên 7 điểm.
SELECT * 
FROM DIEMSV 
WHERE DIEM > 7
----13. Liệt tên môn học cùng số lượng SV đã học và đã có điểm.
SELECT T1.MAMH,T.TENMH, count (T1.MSSV) 
FROM MONHOC T, DIEMSV T1
WHERE T.MAMH = T1.MAMH
GROUP BY T1.MAMH,T.TENMH
----14. Liệt kê tên SV và điểm trung bình của SV đó.
SELECT HOTEN, AVG(T2.DIEM)
FROM SINHVIEN T1, DIEMSV T2
WHERE T1.MSSV = T2.MSSV
GROUP BY T1.MSSV, HOTEN
----15. Liệt kê tên sinh viên đạt điểm cao nhất của môn học ‘Kỹ thuật lập trình’.
SELECT HOTEN
FROM SINHVIEN T1, DIEMSV T2
WHERE T1.MSSV = T2.MSSV and MAMH = 'COS201' and DIEM >= ( select max(DIEM) from DIEMSV)