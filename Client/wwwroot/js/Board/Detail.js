// // get board detail by id
// $(document).ready(function () {
//    $.ajax({
//       type: "GET",
//       url: `https://localhost:5001/api/boards/detail/${}`,
//       data: "data",
//       dataType: "dataType",
//       success: function (response) {

//       }
//    });
// });

const getBoardDetailById = (id) => {
   $.ajax({
      type: 'GET',
      url: `https://localhost:5001/api/boards/detail/${id}`,
      dataType: 'json',
      success: function (response) {
         $('#title').html(response.data.name)
         $('#description').html(response.data.description)
         $('#createdAt').html(moment(response.data.createdAt).format('LLL'))

         console.log(response)
      },
      error: function (e) {
         location.href = '/boards'
      },
   })
}
