// 'using' statements
import "babel-polyfill"
import fetch from "isomorphic-fetch"
import React, {Component} from 'react'
import {render} from 'react-dom'
import { Router, Route, Link, browserHistory, hashHistory } from 'react-router'


// const getMessage = async (id) => {
//     let response = await fetch(`/api/message/${id}`)
//     let data = await response.json()
    
//     console.log(data)
// }

// const app = () => {
//     getMessage(4);
// }

// app();

const log = (...a) => console.log(...a)

const get = (url) =>
    fetch(url, {credentials: 'same-origin'})
    .then(r => {
        if (r.status === 200) return r.json()

        throw 403
    })
    .catch(e => {
        if(e === 403) window.location.hash = "#login"
    })

const post = (url, data) => 
    return fetch(url, { 
        method: 'POST',
        credentials: 'same-origin',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(data)
    })
    .catch(e => log(e))
    .then(r => r.json())

const room = x =>
    <Link to={`/room/${x.id}`} className="room">
        <p>{x.name}</p>
    </Link>



const Menu = () => <div>
    <a href="#">Home</a>
</div>
class Home extends Component {
    constructor(p){
        super(p)
        this.state = {
            rooms: []
        }
    }
    componentDidMount() {
        get(`/api/room`).then(rooms => this.setState({rooms}))
    }
    submit(e) {
        e.preventDefault();
        post('/api/room', { name: this.refs.name.value })
            .then(x => {
                const {rooms} = this.state
                rooms.push(x)
                this.setState({room})
            })
            .catch(e => log(e))
    }
    render(){
        return <div>
            <Menu />
            <hr/>
            <form onSubmit={e => this.submit(e)}>
                <div>
                    <label htmlFor="name">Name of new Terd</label>
                    <input ref="name" type="text" id="name" placeholder="Type a name of the new Terd" />
                </div>
                <button type="submit">Send</button>
            </form>
            <hr/>
            <div>
                {this.state.rooms.map(room)}
            </div>
        </div>
    }
}

const message = m =>
    <li>{m.text} - <em>{m.user.name}</em></li>

class Room extends Component {
    constructor(p){
        super(p)
        this.state = { room: {} }
    }
    componentDidMount() {
        get(`/api/room/${this.props.params.roomId}`)
            .then(room => this.setState({room}))
    }
    // componentDidMount() {
    //     get(`/api/user/${this.props.params.user.id}`)
    //         .then(user => this.setState({user}))
    // }

    submit(e){
        e.preventDefault()
        post('/api/message', { text: this.refs.message.value, roomid: this.state.room.id, user: this.user.id})
            .then(x => {
                const {room} = this.state
                room.messages.push(x)
                this.setState({room})
            })
            .catch(e => alert(e))
            
    }
    render(){
        const {room} = this.state
        return <div>
            <Menu />          
            <hr />
            <h5>{room.name}</h5>
            <hr/>
            <form onSubmit={e => this.submit(e)}>
                <div>
                    <label htmlFor="message">Message</label>
                    <input ref="message" type="text" id="message" placeholder="Type a message to terd" />
                </div>
                <button type="submit">Send</button>
            </form>
            <hr/>
            {room.messages && room.messages.map(message)}
        </div>
    }
}

class Login extends Component {
    constructor(p){
        super(p)
    }
    submit(e){
        e.preventDefault()
        post('/login', 
            {
                email: this.refs.email.value,
                password: this.refs.password.value
            })
            .then(x => {
                window.location.hash = "#"
            })
            .catch(e => log(e))
    }
    render(){
        return <div>
            <Menu />
            <hr/>
            <form onSubmit={e => this.submit(e)}>
                <div>
                    <label htmlFor="email">Email</label>
                    <input ref="email" type="email" id="email" placeholder="Your email" />
                </div>
                <div>
                    <label htmlFor="password">Password</label>
                    <input ref="password" type="password" id="password" placeholder="Your password" />
                </div>
                <button type="submit">Login</button>
            </form>
        </div>
    }
}

const a404 = () => <h2> Page Not Found! U mad? </h2>

const reactApp = () => 
    render(
    <Router history={hashHistory}>
        <Route path="/" component={Home}/>
        <Route path="/room/:roomId" component={Room}/>
        <Route path="/login" component={Login}/>
        <Route path="*" component={a404}/>
    </Router>,
    document.querySelector('.app'))
reactApp()

window.user=user;