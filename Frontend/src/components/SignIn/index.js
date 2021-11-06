import React from 'react';
import { Redirect } from 'react-router';
import {
  Container,
  FormWrap,
  Icon,
  FormContent,
  Form,
  FormH1,
  FormH2,
  FormLabel,
  FormInput,
  FormButton,
  Text
} from './SigninElements';

class SignIn extends React.Component {

    constructor (props) { 
        super(props);

        this.state = {
            redirect: null,
            message: "",
            email: "",
            password: "",
            user: {},
        };
    
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        // console.log(props);
      }
    
      handleChange = event => {
        this.setState({
            [event.target.name]: event.target.value
        });
      }
    
      handleSubmit(e) {
        console.log(e);
        e.preventDefault();
        this.callAPI();
      }
    
      formValidation() {
        return this.state.password.length > 0 && this.state.email.length > 0;
      }

      callAPI(){
        let API = "https://localhost:44347/api/";
        let query = "getToken";
        fetch(API + query, {
            method: 'POST',
            mode: 'cors', 
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({"email": this.state.email, "password": this.state.password})
        })
        .then(response => {
          if (response.ok) {
            response.json().then(json => {
              this.setState({user: json}, () => {
                this.getUser()
              });
              // console.log(this.state.user);
              // window.localStorage.setItem("user", JSON.stringify(json));
            });
            // window.localStorage.setItem("user", this.state.user);
            // this.setState({user:JSON.parse(window.localStorage.getItem("user"))});
            // this.getUser();
            // console.log(this.state.user);
            this.setState({redirect: "/"});
          }
          else {
            response.json().then(json => this.setState({message: json["message"]}));
          }
        })
      }

      getUser() {
        let API = "https://localhost:44347/api/";
        let query = "getUser";
        console.log(this.state.user);
        fetch(API + query, {
            method: 'POST',
            mode: 'cors',
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + this.state.user["token"]
            },
            body: JSON.stringify({"UserId": this.state.user["userId"]})
        })
        .then(response => response.json())
        .then(json => {
            const newUser = {...this.state.user, "accounts": json.accounts};
            window.localStorage.setItem("user", JSON.stringify(newUser));
            this.setState({user:newUser});
        });        
      };  

render() {
    if (this.state.redirect) {
      return <Redirect to={this.state.redirect} />
    }
    return (
        <>
          <Container>
            <FormWrap>
              <Icon to='/'>Main Page</Icon>
              <FormContent>
                <Form onSubmit = {(e) => this.handleSubmit(e)}>
                  <FormH1>Sign in to your account</FormH1>
                  <FormLabel htmlFor='email'>Email</FormLabel>
                  <FormInput name="email" value={this.state.email} onChange = {this.handleChange} type='email' placeholder="Enter Email" required />
                  <FormLabel htmlFor='password'>Password</FormLabel>
                  <FormInput name="password" value={this.state.password} onChange= {this.handleChange} type='password' placeholder="Enter Password" required />
                  <FormButton type='submit'>Continue</FormButton>
                  <FormH2>{this.state.message}</FormH2>
                  <Text onClick={this.getTransaction}>Forgot password</Text>
                </Form>
              </FormContent>
            </FormWrap>
          </Container>
        </>
      );
    }
}
/*
start a function to handle login


handleLogin = (event) => {
    event.preventDefault();
    const { email, password } = this.state;
    const fieldsToValidate = [{ email }, { password }];

    const allFieldsEntered = validateFields(fieldsToValidate);
    if (!allFieldsEntered) {
      this.setState({
        errorMsg: {
          signin_error: 'Please enter all the fields.'
        }
      });
    } else {
      this.setState({
        errorMsg: {
          signin_error: ''
        }
      }); 
      // login successful
    }
  };

const SignIn = () => {
  return (
    <>
      <Container>
        <FormWrap>
          <Icon to='/'>Main Page</Icon>
          <FormContent>
            <Form action='#'>
              <FormH1>Sign in to your account</FormH1>
              <FormLabel htmlFor='email'>Email</FormLabel>
              <FormInput name="email" type='email' placeholder="Enter Email" required />
              <FormLabel htmlFor='password'>Password</FormLabel>
              <FormInput name="password" type='password' placeholder="Enter Password" required />
              <FormButton type='submit'>Continue</FormButton>
              <Text>Forgot password</Text>
            </Form>
          </FormContent>
        </FormWrap>
      </Container>
    </>
  );
};

*/

export default SignIn;
