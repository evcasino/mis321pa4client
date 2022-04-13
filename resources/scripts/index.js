//const baseUrl = "https://localhost:5001/api/SongsCon";
const baseUrl = "https://mis321pa4evc.herokuapp.com/api/SongsCon";
var playlist = [];

function populateList(){
    const url = baseUrl; 
    fetch(url).then(function(response) {
		console.log(response);
		return response.json();
	}).then(function(json) {
        playlist = json;
        console.log(playlist);
        let html = '';
        let rid = "";
        json.forEach((Song) => {
            
            console.log(Song);
            rid = Song.songID;
            console.log(rid);
            html += `<div class="card col-md-4 bg-dark text-white">`;
			html += `<img src="./resources/images/music.jpeg" class="card-img" alt="...">`;
			html += `<div class="card-img-overlay">`;
			
            if (Song.favorite == "n")
            {
                html += `<h5 class="card-title" >`+Song.songTitle +`</h5>`;
                html += '<button id= '+Song.songID+' class="btn btn-success" onclick="favorite(id)">Favorite</button>';
                html += '<button id= '+Song.songID+' class="btn btn-danger" onclick="deleteSong(id)">Delete</button>';
            }
            else if (Song.favorite == "y")
            {
                html += `<h5 class="card-title">`+Song.songTitle + "⭐" +`</h5>`;
                html += '<button id= '+Song.songID+' class="btn btn-warning" id = "selectId" onclick="unfavorite(id)">Unfavorite</button>';
                //html += `<option value = ${Song.songID}> ${Song.songID} </option> `
            }
            html += `</div>`;
            html += `</div>`;
            //console.log(document.getElementById("faveId").value);
		});
        document.getElementById("allSongs").innerHTML = html;
}).catch(function(error) {
    console.log(error);
})

}
var Song = [];
function addSongs(){
    const url = baseUrl;
    console.log(url)
    const today = new Date();
    const Deleted = "n";
    const sendSong = {
        
        songTitle: document.getElementById("addSong").value,
        deleted: Deleted,

    }

    fetch(url, {
        method: "POST",
        headers: {
            "Accept": 'application/json',
            "Content-Type": 'application/json',
        },
        body: JSON.stringify(sendSong)
    }).then((response)=>{
        populateList();
        console.log(response);
    })
}

function favorite(id){
    const url = baseUrl;
    console.log(id + " testID favorite method");
    const Deleted = "n";
    const Favorite = "y";
    const sendSong = {
        deleted: Deleted,
        favorite: Favorite,

    }
    console.log(sendSong);

    fetch(url + "/" + id, {
        method: "PUT",
        headers: {
            "Accept": 'application/json',
            "Content-Type": 'application/json',
        },
        body: JSON.stringify(sendSong)
    }).then((response)=>{
        populateList();
    })
}
function unfavorite(id){
    const url = baseUrl;
    console.log(id + " testID favorite method");
    const Deleted = "n";
    const Favorite = "n";
    const SongTitle = " ";
    const SongTimestamp = "";

    //url += searchString;

    //console.log(searchString)
    const sendSong = {
        deleted: Deleted,
        favorite: Favorite,

    }
    console.log(sendSong);

    fetch(url + "/" + id, {
        method: "PUT",
        headers: {
            "Accept": 'application/json',
            "Content-Type": 'application/json',
        },
        body: JSON.stringify(sendSong)
    }).then((response)=>{
        //Song = sendSong;
        populateList();
    })
}
function deleteSong(id){ //Idek where to start
    // var url = "https://mis321pa4evc.herokuapp.com/api/Songs/" + "/" + id;
    //let searchString = document.getElementById("deleteSong").value;
    const url = baseUrl;

    fetch(url + "/" + id, {
        method: "DELETE",
        headers: {
            "Accept": 'application/json',
            "Content-Type": 'application/json',
        }
    }).then((response)=>{
        //Song = sendSong;
        populateList();
    })
}