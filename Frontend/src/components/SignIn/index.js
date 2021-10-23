import React from 'react';
import {
  Container,
  FormWrap,
  Icon,
  FormContent,
  Form,
  FormH1,
  FormLabel,
  FormInput,
  FormButton,
  Text
} from './SigninElements';

class SignIn extends React.Component {

    constructor (props) { 
        super(props);
    
        this.state = {
            email: "",
            password: "",
            hits: [],
        };
    
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
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
        .then(response => response.json())
        .then(data => this.setState({ hits: data.hits }));
      };

render() {
    
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
                  <Text>Forgot password</Text>
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
