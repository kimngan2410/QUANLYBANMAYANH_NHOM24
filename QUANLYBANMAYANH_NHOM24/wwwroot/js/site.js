document.addEventListener("DOMContentLoaded", function () {
    console.log("DOM đã tải xong");

    // Hàm cập nhật giao diện người dùng dựa trên trạng thái từ server
    function updateUserUI(data) {
        const userAction = document.querySelector(".user-action");

        if (!userAction) return;

        if (data.isLoggedIn) {
            console.log("Người dùng đã đăng nhập:", data.userName);
            userAction.innerHTML = `
            <div class="dropdown">
                <a href="#" id="userDropdown">
                    <img src="${data.avatarUrl}" alt="Avatar" class="rounded-circle" width="40" height="40" data-logged-in="true">
                </a>
                <ul class="dropdown-menu" aria-labelledby="userDropdown">
                    <li><a class="dropdown-item" href="/KhachHang/CapNhatThongTin">Tài khoản của tôi</a></li>
                    <li><a class="dropdown-item" href="#">Đơn hàng</a></li>
                    <li><a class="dropdown-item text-danger" href="/Login/DangXuat">Đăng xuất</a></li>
                </ul>
            </div>
            `;

            // Lấy các phần tử cần thiết
            const userDropdown = document.getElementById("userDropdown");
            const dropdownMenu = userAction.querySelector(".dropdown-menu");

            if (userDropdown && dropdownMenu) {
                // Bật/tắt dropdown khi nhấn vào avatar
                userDropdown.addEventListener("click", function (event) {
                    event.preventDefault(); // Ngăn reload trang
                    dropdownMenu.classList.toggle("show");
                });

                // Đóng dropdown khi click ra ngoài
                document.addEventListener("click", function (event) {
                    // Nếu click không nằm trong dropdown
                    if (!userAction.contains(event.target)) {
                        dropdownMenu.classList.remove("show");
                    }
                });
            }
        } else {
            console.log("Người dùng chưa đăng nhập");
            userAction.innerHTML = `
            <a href="#" data-bs-toggle="modal" data-bs-target="#LoginMenu">
                <img class="icon-user" src="/images/user.png" alt="Icon User" data-logged-in="false">
            </a>
        `;
        }
    }



    // Hàm thiết lập sự kiện click cho user action
    function setupUserActions(userImg) {
        if (!userImg) return;

        userImg.addEventListener("click", function (event) {
            event.stopPropagation(); // Ngăn sự kiện click lan ra bên ngoài

            const isLoggedIn = userImg.getAttribute("data-logged-in") === "true";

            if (isLoggedIn) {
                console.log("Hiển thị Dropdown Menu");

                const dropdown = userImg.closest(".dropdown");
                if (dropdown) {
                    const dropdownMenu = dropdown.querySelector(".dropdown-menu");
                    if (dropdownMenu) {
                        // Toggle class "show"
                        const isShown = dropdownMenu.classList.contains("show");
                        document.querySelectorAll(".dropdown-menu").forEach(menu => menu.classList.remove("show")); // Đóng các dropdown khác
                        if (!isShown) dropdownMenu.classList.add("show");

                        // Đóng menu khi click bên ngoài
                        document.addEventListener("click", function (event) {
                            if (!dropdown.contains(event.target)) {
                                dropdownMenu.classList.remove("show");
                            }
                        }, { once: true });
                    }
                }
            } else {
                console.log("Hiển thị LoginMenu");
                const loginMenu = new bootstrap.Modal(document.getElementById("LoginMenu"));
                loginMenu.show();
            }
        });
    }

    function updateUserInterface(data) {
        const userImg = document.querySelector(".user-action img");

        if (data.isLoggedIn) {
            console.log("Người dùng đã đăng nhập:", data.userName);
            userImg.src = data.avatarUrl;
            userImg.setAttribute("data-logged-in", "true");
            userImg.classList.remove("icon-user");
        } else {
            console.log("Người dùng chưa đăng nhập");
            userImg.src = "/images/user.png";
            userImg.setAttribute("data-logged-in", "false");
            userImg.classList.add("icon-user");
        }

        setupUserActions(userImg);
    }

    // Hàm kiểm tra trạng thái đăng nhập từ server
    function checkLoginStatus() {
        fetch('/Login/GetLoginStatus')
            .then(response => response.json())
            .then(data => {
                console.log("Trạng thái đăng nhập từ server:", data);
                updateUserUI(data);

                // Nếu đã đăng nhập, đóng modal LoginMenu
                if (data.isLoggedIn) {
                    const loginMenuModal = bootstrap.Modal.getInstance(document.getElementById("LoginMenu"));
                    if (loginMenuModal) {
                        loginMenuModal.hide();
                    }
                }
            })
            .catch(error => {
                console.error("Lỗi khi kiểm tra trạng thái đăng nhập:", error);
            });
    }

    // Kiểm tra trạng thái đăng nhập ngay khi tải DOM
    checkLoginStatus();

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
            event.preventDefault();

            const errorSummary = document.getElementById("errorSummary");
            if (errorSummary) errorSummary.innerText = "";

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

            const formData = new FormData(loginForm);

            fetch(loginForm.action, {
                method: "POST",
                body: formData,
            })
                .then(response => {
                    if (!response.ok) throw new Error("Network response was not ok");
                    return response.json();
                })
                .then(data => {
                    console.log("Server response:", data);

                    if (data.success) {
                        console.log("Đăng nhập thành công!");
                        checkLoginStatus();
                        const loginFormModal = bootstrap.Modal.getInstance(document.getElementById("LoginForm"));
                        loginFormModal.hide();
                    } else {
                        if (errorSummary) {
                            errorSummary.innerText = data.message;
                        }
                    }
                })
                .catch(error => {
                    console.error("Có lỗi xảy ra:", error);
                    if (errorSummary) {
                        errorSummary.innerText = "Đã xảy ra lỗi không mong muốn. Vui lòng thử lại.";
                    }
                });
        });
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

    

    checkLoginStatus();
});