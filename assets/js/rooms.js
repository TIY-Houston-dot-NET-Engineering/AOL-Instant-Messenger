// 'using' statements
import "babel-polyfill"
import fetch from "isomorphic-fetch"
import React, {Component} from 'react'
import {render} from 'react-dom'
import { Router, Route, Link, browserHistory, hashHistory } from 'react-router'
import * as BLUE from '@blueprintjs/core'

// Utility methods
// --------------
const log = (...a) => console.log(...a)

const get = (url) =>
    fetch(url, {credentials: 'same-origin'})
    .then(r => r.json())
    .catch(e => log(e))

const post = (url, data) => 
   
    fetch(url, { 
        method: 'POST',
        // credentials: 'same-origin',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(data)
    })
    .catch(e => log(e))
    .then(r => r.json())

const destroy = (url, data) =>
    fetch(url, {
        method: 'DELETE',
        // credentials: 'same-origin'
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(data)
    })
    .delete(data)
    
// ----------------

// const room = r =>
// <Link to={'/room/${x.id}'} className="room">
//     <h5>{r.name}</h5>
//     <p>{r.summary}</p>
// </Link>

const Nav = () => 
    <nav className="pt-navbar pt-dark pt-fixed-top">
        <div className="pt-navbar-group pt-align-left">
            <div className="pt-navbar-heading">Blueprint</div>
            <input className="pt-input" placeholder="Search files..." type="text" />
        </div>
        <div className="pt-navbar-group pt-align-right">
            <button className="pt-button pt-minimal pt-icon-home"><a href="/">Home</a></button>
            <button className="pt-button pt-minimal pt-icon-document">Rooms</button>
            <span className="pt-navbar-divider"></span>
            <button className="pt-button pt-minimal pt-icon-user"></button>
            <button className="pt-button pt-minimal pt-icon-notifications"></button>
            <button className="pt-button pt-minimal pt-icon-cog"></button>
        </div>
    </nav>


const RoomCard = ({name="IM DA BOSS", summary="and you ain't", url="#"}) => 
    <div className="pt-card pt-elevation-1 pt-interactive">
        <h5><a href={url}>{name}</a></h5>
        <p>{summary}</p>
        <div>
            <button type="submit">Edit Room</button> | <button type="submit">Delete Room</button>
        </div>
    </div>

class Room extends Component {
    constructor(props){
        super(props)
        this.removeRoom = this.removeRoom.bind(this)
        this.state = {
            rooms: []
        }
    }

    componentDidMount() {
        get('api/room')
        .then(rooms => this.setState({rooms}))
        .catch(e => log(e))
    }

    removeRoom(key){
        const data = {rooms};
        delete data[key];
        this.setState({data})
    }

    // removeRoom(key){
    //     remove('api/room/${key}')
    //     .then(rooms => this.setState({rooms}))
    // }

    // removeRoom(e) {
    //     var arr = this.state.rooms.filter(function(item) {
    //         return item != e.target.value
    //     })
    //     this.setState({
    //         rooms: arr
    //     })
    // }

    render() {
        console.log(this.state)
        
        return (
            <div>
                <Nav />
                <hr />
                <h2>Rooms:</h2>
                <div className="grid grid-3-600">
                    {Object
                        .keys(this.state.rooms)
                        .map(key => <RoomCard key={key} name={this.state.rooms[key].name} summary={this.state.rooms[key].summary}/>)}
                </div>
            <NewRoom/>
            </div>
        )
    }
}


class NewRoom extends Component {
    constructor(props){
        super(props)
        this.state = {}
    }
    submit(e){
        e.preventDefault()
        post('/api/room', {
            name: this.refs.name.value,
            summary: this.refs.summary.value
        }).then(x => {
            if(!x.errors) window.location.hash = `#/room/`

            this.setState({ errors: x.errors })
        }).catch(e => alert(e))
    }
    
    render(){
        var err 
        if(this.state.errors){
            err = <ul className="compose-errors">
                    {this.state.errors.map(x => <li>{x}</li>)}
                </ul>
        }

        return <form className="compose-screen" onSubmit={e => this.submit(e)}>

            {this.state.errors ? <p>There were errors creating the room:</p> : null}
            {err}

            <div>
            <hr/>
                <h4>Add New Room:</h4>
                <label ref="name">Name of Room:</label><br/>
                <input ref="name" type="text" placeholder="Room Name" required /><br/>
                <label ref="summary">Room Description:</label><br/>
                <textarea ref="summary" placeholder="Room Description" required></textarea>
            </div>
            <div>
                <button type="submit">Create Room</button>
            </div>
        </form>
    }
}

export default Room
