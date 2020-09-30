var tokenKey = "accessToken";


    async function getTokenAsync() {

        var user = {
            Login: document.getElementById("loginLogin").value,
            Password: document.getElementById("passwordLogin").value
        };
        var serializedUser = JSON.stringify(user);


        const response = await fetch("/account/login", {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: serializedUser
        });

        const data = await response.json();


        if (response.ok === true) {
            document.getElementById("userName").innerText = data.username;
            document.getElementById("roleName").innerText = data.role;
            document.getElementById("userInfo").style.display = "block";
            document.getElementById("loginForm").style.display = "none";
            document.getElementById("registerForm").style.display = "none";
            document.getElementById("deleteUserdiv").style.display = "block";
            document.getElementById("getUsersdiv").style.display = "block";
            document.getElementById("createUserbutton").style.display = "block";
            document.getElementById("updateUserbutton").style.display = "block";
            sessionStorage.setItem(tokenKey, data.access_token);
            console.log(data.access_token);
        }
        else {
            console.log("Error: ", response.status, data.errorText);
            document.getElementById("ErrorLogin").innerText = data.errorText;
        }
    };
    async function RegisterAsync() {
        var user =
        {
            Login: document.getElementById("registerLogin").value,
            Password: document.getElementById("registerPassword").value,
            ConfirmPassword: document.getElementById("registerconfirmPassword").value,
            Name: document.getElementById("registerName").value,
            Email: document.getElementById("registerEmail").value
        };
        var serializedUser = JSON.stringify(user);
        const response = await fetch("/account/register",
            {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                body: serializedUser
            });
        const data = await response.json();
        if (response.ok === true) {
            alert(data.text);
        }
        else {
            console.log("Error: ", response.status, data.errorText);
            document.getElementById("ErrorRegister").innerText = data.errorText;
        }
    };
    async function CreateAsync() {
        const token = sessionStorage.getItem(tokenKey);
        var user =
        {
            Login: document.getElementById("createLogin").value,
            Password: document.getElementById("createPassword").value,
            Name: document.getElementById("createName").value,
            Email: document.getElementById("createEmail").value,
            Role: document.getElementById("createRole").valueAsNumber
        }
        var serializedUser = JSON.stringify(user);
        const response = await fetch("api/test",
            {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + token
                },
                body: serializedUser
            });
        const data = await response.json();
        if (response.ok === true) {
            alert("Пользователь создан");
        }
        else {
            console.log("Error: ", response.status, data.errorText);
            alert(data.errorText);
        }
    };
    async function UpdateAsync() {
        const token = sessionStorage.getItem(tokenKey);
        var user =
        {
            Id: document.getElementById("updateId").valueAsNumber,
            Login: document.getElementById("updateLogin").value,
            Password: document.getElementById("updatePassword").value,
            Name: document.getElementById("updateName").value,
            Email: document.getElementById("updateEmail").value,
            Role: document.getElementById("updateRole").valueAsNumber
        }
        var serializedUser = JSON.stringify(user);
        const response = await fetch("api/test",
            {
                method: "PUT",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + token
                },
                body: serializedUser
            });
        const data = await response.json();
        if (response.ok === true) {
            alert("Пользователь обновлен");
        }
        else {
            console.log("Error: ", response.status, data.errorText);
            alert(data.errorText);
        }
    };
    async function getData(url) {
        const token = sessionStorage.getItem(tokenKey);

        const response = await fetch(url, {
            method: "GET",
            headers: {
                "Accept": "application/json",
                "Authorization": "Bearer " + token
            }
        });
        if (response.ok === true) {
            const data = await response.json();
            let users = [];
            users = JSON.parse(data);
            document.getElementById("listUsers").style.display = "block";
            document.getElementById("listUsers").innerHTML = '<ul start="0">';
            users.forEach(user => {
                document.getElementById("listUsers").innerHTML += `<li>Id: ${user.Id} Имя:<span style="color: #1a55cc"> ${user.Name}</span> Логин: ${user.Login}
                        Email: ${user.Email} Роль: ${user.Role}</li>`;
            });
            document.getElementById("listUsers").innerHTML += '<ul>';
        }
        else
            console.log("Status: ", response.status);
    };
    async function deleteUser(url, id) {
        const token = sessionStorage.getItem(tokenKey);
        const response = await fetch(url + "/" + id, {
            method: "DELETE",
            headers: {
                "Accept": "application/json",
                "Authorization": "Bearer " + token
            }
        });
        const data = await response.json();
        if (response.ok === true) {
            alert(data.text);
        }
        else {
            console.log("Status: ", response.status);
            alert(data.errorText);
        }
    };


    document.getElementById("submitLogin").addEventListener("click", e => {

        e.preventDefault();
        getTokenAsync();
    });

    document.getElementById("submitRegister").addEventListener("click", e => {

        e.preventDefault();
        RegisterAsync();
    });


    document.getElementById("logOut").addEventListener("click", e => {

        e.preventDefault();
        document.getElementById("userName").innerText = "";
        document.getElementById("userInfo").style.display = "none";
        document.getElementById("loginForm").style.display = "block";
        document.getElementById("registerForm").style.display = "block";
        document.getElementById("deleteUserdiv").style.display = "none";
        document.getElementById("getUsersdiv").style.display = "none";
        document.getElementById("createUserbutton").style.display = "none";
        document.getElementById("createUserdiv").style.display = "none";
        document.getElementById("updateUserbutton").style.display = "none";
        document.getElementById("updateUserdiv").style.display = "none";
        document.getElementById("listUsers").style.display = "none";
        sessionStorage.removeItem(tokenKey);
    });



    document.getElementById("getUsers").addEventListener("click", e => {
        e.preventDefault();
        getData("/api/test");
    });


    document.getElementById("deleteUser").addEventListener("click", e => {
        var id = prompt("Введите ID удаляемого пользователя");
        e.preventDefault();
        deleteUser("/api/test", id);
    });

    document.getElementById("createUserbutton").addEventListener("click", e => {
        e.preventDefault();
        document.getElementById("createUserdiv").style.display = "block";
        document.getElementById("submitCreate").addEventListener("click", e => {
            e.preventDefault();
            CreateAsync();
        });
    });
    document.getElementById("updateUserbutton").addEventListener("click", e => {
        e.preventDefault();
        document.getElementById("updateUserdiv").style.display = "block";
        document.getElementById("submitUpdate").addEventListener("click", e => {
            e.preventDefault();
            UpdateAsync();
        });
    });