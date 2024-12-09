document.addEventListener("DOMContentLoaded", function () {
    console.log("DOM đã tải xong");

    // Hiển thị LoginMenu khi nhấn vào icon user
    const iconUser = document.querySelector(".icon-user");
    if (iconUser) {
        iconUser.addEventListener("click", function () {
            console.log("Icon User được nhấn");
            const loginMenu = new bootstrap.Modal(document.getElementById("LoginMenu"));
            loginMenu.show();
        });
    }

    // Hiển thị LoginForm khi nhấn "Đăng nhập" trong LoginMenu
    const btnOpenLoginForm = document.querySelector("#btnOpenLoginForm");
    if (btnOpenLoginForm) {
        btnOpenLoginForm.addEventListener("click", function () {
            const loginMenu = bootstrap.Modal.getInstance(document.getElementById("LoginMenu"));
            loginMenu.hide(); // Ẩn LoginMenu

            const loginFormModal = new bootstrap.Modal(document.getElementById("LoginForm"));
            loginFormModal.show(); // Hiển thị LoginForm
        });
    }

    // Hiển thị RegisterForm khi nhấn nút "Đăng ký tài khoản"
    const btnOpenRegisterForm = document.querySelector("#btnOpenRegisterForm");
    if (btnOpenRegisterForm) {
        btnOpenRegisterForm.addEventListener("click", function () {
            const loginMenu = bootstrap.Modal.getInstance(document.getElementById("LoginMenu"));
            loginMenu.hide(); // Ẩn LoginMenu

            const registerFormModal = new bootstrap.Modal(document.getElementById("RegisterForm"));
            registerFormModal.show(); // Hiển thị RegisterForm
        });
    }

    // Gắn sự kiện "submit" cho form trong modal
    const loginForm = document.getElementById("loginForm");
    if (loginForm) {
        loginForm.addEventListener("submit", function (event) {
            event.preventDefault(); // Ngăn form gửi đi theo cách mặc định

            // Kiểm tra dữ liệu trước khi gửi
            const errorSummary = document.getElementById("errorSummary");
            if (errorSummary) errorSummary.innerText = ""; // Xóa lỗi cũ

            const email = document.getElementById("Email").value.trim();
            const password = document.getElementById("MatKhau").value.trim();

            if (!email) {
                if (errorSummary) errorSummary.innerText = "Email không được để trống.";
                return;
            }

            if (!password) {
                if (errorSummary) errorSummary.innerText = "Mật khẩu không được để trống.";
                return;
            }

            // Lấy dữ liệu từ form
            const formData = new FormData(loginForm);

            // Gửi AJAX
            fetch(loginForm.action, {
                method: "POST",
                body: formData,
            })
                .then((response) => {
                    if (!response.ok) throw new Error("Network response was not ok");
                    return response.json();
                })
                .then((data) => {
                    console.log("Server response:", data); // In kết quả trả về từ server

                    if (data.success) {
                        window.location.href = data.redirectUrl;
                    } else {
                        const errorSummary = document.getElementById("errorSummary");
                        if (errorSummary) {
                            errorSummary.innerText = data.message;
                        }
                    }
                })
                .catch((error) => {
                    console.error("Có lỗi xảy ra:", error);
                    const errorSummary = document.getElementById("errorSummary");
                    if (errorSummary) {
                        errorSummary.innerText = "Đã xảy ra lỗi không mong muốn. Vui lòng thử lại.";
                    }
                });
        });
    } else {
        console.error("Không tìm thấy form đăng nhập trong modal.");
    }

    // Gắn sự kiện nhập vào trong registerForm
    const registerForm = document.getElementById("registerForm");
    if (registerForm) {
        registerForm.addEventListener("submit", function (event) {
            event.preventDefault(); // Ngăn form gửi theo cách mặc định
            console.log("Đang gửi form đăng ký...");

            // Xóa lỗi cũ trước khi gửi
            const errorFields = document.querySelectorAll("#registerForm .text-danger");
            errorFields.forEach((field) => (field.innerText = ""));

            const formData = new FormData(registerForm);

            fetch(registerForm.action, {
                method: "POST",
                body: formData,
            })
                .then((response) => {
                    console.log("Phản hồi từ server:", response);
                    if (!response.ok) throw new Error("Phản hồi không hợp lệ");
                    return response.json();
                })
                .then((data) => {
                    console.log("Kết quả JSON:", data);
                    if (data.success) {
                        alert(data.message); // Hiển thị thông báo thành công!
                        // Reset form nếu cần
                        registerForm.reset();
                    } else {
                        // Hiển thị lỗi cụ thể từng trường
                        if (data.errors) {
                            data.errors.forEach((error) => {
                                const field = document.querySelector(`#registerForm [name="${error.field}"]`);
                                if (field) {
                                    const errorDiv = field.parentElement.querySelector(".text-danger");
                                    if (errorDiv) errorDiv.innerText = error.message;
                                }
                            });
                        } else {
                            alert(data.message); // Hiển thị lỗi chung
                        }
                    }
                })
                .catch((error) => {
                    console.error("Có lỗi xảy ra:", error);
                    alert("Đã xảy ra lỗi không mong muốn. Vui lòng thử lại.");
                });
        });
    }

    // Đóng LoginForm và xóa lớp nền mờ
    const loginFormModal = document.getElementById("LoginForm");
    if (loginFormModal) {
        loginFormModal.addEventListener("hidden.bs.modal", function () {
            // Đảm bảo xóa tất cả lớp nền mờ
            const modalBackdrops = document.querySelectorAll(".modal-backdrop");
            modalBackdrops.forEach((backdrop) => backdrop.remove());

            // Xóa lớp `modal-open` khỏi body
            document.body.classList.remove("modal-open");

            console.log("LoginForm đã được đóng và lớp nền mờ đã bị xóa.");
        });
    }

    // Đóng LoginMenu và xóa lớp nền mờ
    const loginMenuModal = document.getElementById("LoginMenu");
    if (loginMenuModal) {
        loginMenuModal.addEventListener("hidden.bs.modal", function () {
            // Đảm bảo xóa tất cả lớp nền mờ
            const modalBackdrops = document.querySelectorAll(".modal-backdrop");
            modalBackdrops.forEach((backdrop) => backdrop.remove());

            // Xóa lớp `modal-open` khỏi body
            document.body.classList.remove("modal-open");

            console.log("LoginMenu đã được đóng và lớp nền mờ đã bị xóa.");
        });
    }
});
