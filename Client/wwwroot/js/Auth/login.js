function Login() {
    event.preventDefault();
    var obj = new Object();
    obj.Email = $("#email").val();
    obj.Password = $("#password").val();

    console.log(obj);
}