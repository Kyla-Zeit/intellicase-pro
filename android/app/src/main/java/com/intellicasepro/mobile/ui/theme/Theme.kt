package com.intellicasepro.mobile.ui.theme

import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Shapes
import androidx.compose.material3.Typography
import androidx.compose.material3.lightColorScheme
import androidx.compose.runtime.Composable
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp

val DeepWine = Color(0xFF631225)
val FreshWine = Color(0xFF8B1E31)
val FieldTeal = Color(0xFF0F766E)
val SignalBlue = Color(0xFF2563EB)
val CaseAmber = Color(0xFFD97706)
val AlertRed = Color(0xFFB91C1C)
val Ink = Color(0xFF111827)
val Slate = Color(0xFF64748B)
val AppBackground = Color(0xFFF6F8FB)
val SurfaceSoft = Color(0xFFEFF4F8)

private val LightColors = lightColorScheme(
    primary = DeepWine,
    onPrimary = Color.White,
    secondary = FieldTeal,
    onSecondary = Color.White,
    tertiary = CaseAmber,
    onTertiary = Color.White,
    error = AlertRed,
    background = AppBackground,
    onBackground = Ink,
    surface = Color.White,
    onSurface = Ink,
    surfaceVariant = SurfaceSoft,
    onSurfaceVariant = Slate
)

private val AppShapes = Shapes(
    small = RoundedCornerShape(10.dp),
    medium = RoundedCornerShape(18.dp),
    large = RoundedCornerShape(28.dp)
)

@Composable
fun IntelliCaseTheme(content: @Composable () -> Unit) {
    MaterialTheme(
        colorScheme = LightColors,
        typography = Typography(),
        shapes = AppShapes,
        content = content
    )
}
