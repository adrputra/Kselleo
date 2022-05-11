const getUserId = (Id) => {
    $.ajax({
        url: `https://localhost:44308/api/users/${Id}`,
        type: 'GET',
        success: function (results) {
            let result = results.result

            var fullName = document.getElementById("IdFullName");

            fullName.innerHTML = result.fullName;
            console.log(fullName);
        }
    })
}