import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [CommonModule, ReactiveFormsModule]
})
export class LoginComponent {
  form: FormGroup;
  erroLogin: string | null = null;

  constructor(private fb: FormBuilder, private authService: AuthService,private router: Router) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      senha: ['', Validators.required],
      lembrar: [false]
    });
  }

  login() {
    if (this.form.invalid) return;

    const { email, senha } = this.form.value;

    this.authService.login({ email, senhaDigitada: senha }).subscribe({
      next: (res) => {
        console.log('Login bem-sucedido:', res);
        localStorage.setItem('token', res.token);
        this.router.navigate(['/dashboard/pedidos']);
      },
      error: (err) => {
        console.error('Erro no login:', err);
        this.erroLogin = 'Usuário ou senha inválidos.';
      }
    });
  }
}
